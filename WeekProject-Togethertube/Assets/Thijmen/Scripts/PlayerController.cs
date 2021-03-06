﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof( PlayerInput ) , typeof( CharacterController ) )]
public class PlayerController : MonoBehaviour {

    [Header( "Components" )]
    private CharacterController m_controller;

    [Header( "Movement" )]
    [SerializeField] private float m_maxSpeed = 10f;
    [SerializeField] private float m_acceleration = 10f;
    [SerializeField] private float m_deceleration = 10f;
    [SerializeField] private float m_gravityAcceleration = 9f;
    private Vector2 m_movementInput;
    private Vector3 m_velocity;

    [Header( "Ground Check" )]
    [SerializeField] private float m_groundCheckDistance;
    [SerializeField] private float m_castRadius;
    private RaycastHit m_groundHit;
    private float m_groundAngle;

    [Header( "Look" )]
    [SerializeField] private float m_sensitivity;
    [SerializeField] private Transform m_head;
    private Vector2 m_lookInput;
    private float m_headRot;
    private float m_bodyRot;

    [Header( "Jumping" )]
    [SerializeField] private AnimationCurve m_jumpCurve;
    [SerializeField] private float m_jumpMultiplier;
    [SerializeField] private float m_jumpTime;
    private float m_jumpVelocity;
    private bool m_jumping;

    [Header( "Sliding" )]
    [SerializeField] private float m_slideAccelerateSpeed;
    private float m_slideSpeed;

    [Header( "Hooking" )]
    [SerializeField] CameraFov camfov;
    [SerializeField] Camera cam;
    private float hookshotSize;
    private Transform hookshotCable;
    private HookState state;
    private Vector3 hookshotPos;
    private Vector3 characterVelocityMomentum;
    private const float NOMRAL_FOV = 60f;
    private const float HOOKING_FOV = 120f;


    private enum HookState {
        Normal = 0,
        HookshotThrown,
        HookshotFlyingPlayer,
    }

    private void Start() {
        m_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        state = HookState.Normal;
        hookshotCable = GameObject.Find( "HookshotCable" ).transform;
        camfov = cam.GetComponent<CameraFov>();
        hookshotCable.gameObject.SetActive( false );
    }

    private void Update() {
        switch(state) {
            default:
            case HookState.Normal:
                CheckGround();
                Look( m_lookInput );
                Move( m_movementInput );
                HandleHookshotStart();
                break;
            case HookState.HookshotThrown:
                CheckGround();
                Look( m_lookInput );
                Move( m_movementInput );
                HandleHookshotThrown();
                break;
            case HookState.HookshotFlyingPlayer:
                Look( m_lookInput );
                HandleHookshotMovement();
                break;
        }
    }


    public void OnMovement(InputAction.CallbackContext context) {
        m_movementInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context) {
        m_lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context) {
        if(context.performed && m_controller.isGrounded && !m_jumping && m_groundAngle <= 45f)
            StartCoroutine( Jump() );
    }


    private void Move(Vector2 direction) {
        if(m_controller.isGrounded) {
            // Accelerate And Decelerate
            if(direction.sqrMagnitude > 0.01f) {
                m_velocity.x = Mathf.MoveTowards( m_velocity.x , direction.normalized.x , Time.deltaTime * m_acceleration );
                m_velocity.z = Mathf.MoveTowards( m_velocity.z , direction.normalized.y , Time.deltaTime * m_acceleration );
            } else {
                m_velocity.x = Mathf.MoveTowards( m_velocity.x , 0 , Time.deltaTime * m_deceleration );
                m_velocity.z = Mathf.MoveTowards( m_velocity.z , 0 , Time.deltaTime * m_deceleration );
            }


            if(!m_jumping) {
                // Reset the gravity
                if(m_groundHit.collider != null) {
                    m_velocity.y = -10;

                    if(m_groundAngle > m_controller.slopeLimit) {
                        Vector3 slideDirection = new Vector3( m_groundHit.normal.x , 0 , m_groundHit.normal.z );

                        m_controller.Move( slideDirection * Time.deltaTime * m_slideSpeed );
                        m_slideSpeed += Time.deltaTime * m_slideAccelerateSpeed;

                        // Debug.Log(slideDirection.magnitude);
                        Debug.DrawLine( transform.position , transform.position + slideDirection );
                    } else {
                        m_slideSpeed = 0;
                    }
                } else {
                    m_velocity.y = 0;
                }
            }

        } else {
            m_velocity.y += m_gravityAcceleration * Time.deltaTime;
        }

        // Apply the movement
        Vector3 motion = Quaternion.Euler( 0 , transform.eulerAngles.y , 0 ) * new Vector3( m_velocity.x , 0 , m_velocity.z ) * m_maxSpeed;
        motion.y = m_velocity.y;
        motion += characterVelocityMomentum;
        m_controller.Move( motion * Time.deltaTime );

        if(characterVelocityMomentum.magnitude >= 0f) {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if(characterVelocityMomentum.magnitude < .0f) {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }

    private void Look(Vector2 input) {
        if(input.sqrMagnitude < 0.01f)
            return;

        float mouseX = input.x * m_sensitivity * Time.deltaTime;
        float mouseY = -input.y * m_sensitivity * Time.deltaTime;

        m_bodyRot += mouseX;
        m_headRot += mouseY;

        if(m_bodyRot > 180f)
            m_bodyRot = -180f;
        else
        if(m_bodyRot < -180f)
            m_bodyRot = 180f;

        if(m_headRot > 90f)
            m_headRot = 90f;
        else
        if(m_headRot < -90f)
            m_headRot = -90f;


        Vector3 bodyEuler = transform.eulerAngles;
        bodyEuler.y = m_bodyRot;
        transform.rotation = Quaternion.Euler( bodyEuler );


        Vector3 headEuler = m_head.transform.eulerAngles;
        headEuler.x = m_headRot;
        m_head.rotation = Quaternion.Euler( headEuler );
    }

    private void CheckGround() {

        float distance = (m_controller.height / 2f) + m_groundCheckDistance - m_castRadius;
        if(Physics.SphereCast( transform.position , m_castRadius , Vector3.down , out m_groundHit , distance )) {
            m_groundAngle = Vector3.Angle( m_groundHit.normal , transform.up );
            Debug.DrawLine( transform.position , transform.position + Vector3.down * distance , Color.green , 0.1f );
        }
    }

    private IEnumerator Jump() {
        float timeInAir = 0;
        float oldSlopeLimit = m_controller.slopeLimit;
        float oldStepOffset = m_controller.stepOffset;

        m_controller.slopeLimit = 90f;
        m_controller.stepOffset = 0.01f;
        m_jumping = true;
        m_velocity.y = 0;

        do {
            m_jumpVelocity = m_jumpCurve.Evaluate( timeInAir / m_jumpTime ) * m_jumpMultiplier;
            m_controller.Move( Vector3.up * m_jumpVelocity * Time.deltaTime );

            if((m_controller.collisionFlags & CollisionFlags.Above) != 0)
                break;

            timeInAir += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        while(!(m_controller.isGrounded && m_jumpVelocity + m_velocity.y <= 0));

        m_velocity.y = m_jumpCurve.Evaluate( 1f );
        m_jumping = false;
        m_controller.stepOffset = oldStepOffset;
        m_controller.slopeLimit = oldSlopeLimit;
    }

    private void HandleHookshotStart() {
        if(TestInputDownHookShot()) {
            if(Physics.Raycast( m_head.transform.position , m_head.transform.forward , out RaycastHit raycastHit )) {
                //raycastHit.point
                hookshotPos = raycastHit.point;
                hookshotSize = 0f;
                hookshotCable.gameObject.SetActive( true );
                hookshotCable.localScale = Vector3.zero;
                state = HookState.HookshotThrown;
            }
        }
    }

    private void HandleHookshotThrown() {
        float hookshotThrowSpeed = 100f;

        hookshotCable.LookAt( hookshotPos );
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotCable.localScale = new Vector3( 1 , 1 , hookshotSize );

        if(hookshotSize >= Vector3.Distance( transform.position , hookshotPos )) {
            state = HookState.HookshotFlyingPlayer;
            camfov.SetCameraFov( HOOKING_FOV );
        }
    }

    private void ResetGravityEffect() {
        m_velocity.y = 0;
    }

    private void HandleHookshotMovement() {
        hookshotCable.LookAt( hookshotPos );
        Vector3 hookshotDir = (hookshotPos - transform.position).normalized;

        float hookspeedTravelSpeedMin = 10f;
        float hookspeedTravelSpeedMax = 35f;

        float hookshotTravelSpeed = Mathf.Clamp( Vector3.Distance( transform.position , hookshotPos ) , hookspeedTravelSpeedMin , hookspeedTravelSpeedMax );
        float hookshotTravelSpeedMultiplier = 1.5f;
        float reachedHookshotTarget = 1f;

        m_controller.Move( hookshotDir * hookshotTravelSpeed * hookshotTravelSpeedMultiplier * Time.deltaTime );

        if(Vector3.Distance( transform.position , hookshotPos ) < reachedHookshotTarget) {
            StopHookShot();
        }

        if(TestInputDownHookShot()) {
            StopHookShot();
        }

        if(Input.GetKeyDown( KeyCode.Space )) {
            float momentumExtraSpeed = 4f;
            float jumpSpeed = 30f;
            characterVelocityMomentum = hookshotDir * hookshotTravelSpeed * momentumExtraSpeed;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            StopHookShot();
        }
    }

    private void StopHookShot() {
        state = HookState.Normal;
        ResetGravityEffect();
        hookshotCable.gameObject.SetActive( false );
        camfov.SetCameraFov( NOMRAL_FOV );
    }

    private bool TestInputDownHookShot() {
        return Input.GetKeyDown( KeyCode.E );
    }
}