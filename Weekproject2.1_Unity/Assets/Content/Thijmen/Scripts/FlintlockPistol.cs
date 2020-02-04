using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlintlockPistol : Guns {

    [SerializeField] private Animator shootAnim;
    private bool isLoaded;

    public override void PrimaryAttack(InputAction.CallbackContext context) {
        if(context.performed && isLoaded) {
            shootAnim.SetTrigger( "Shoot" );
            isLoaded = false;
        }
    }

    public override void SecondaryAttack(InputAction.CallbackContext context) {
        if(context.performed) {
            shootAnim.SetTrigger( "Load" );
            isLoaded = true;
        }
    }

}
