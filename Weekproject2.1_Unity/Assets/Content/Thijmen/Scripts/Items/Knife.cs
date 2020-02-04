using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knife : Weapon
{
    public override void PrimaryAttack(InputAction.CallbackContext context)
    {
        if(context.performed) {
            Debug.Log("Wapen Piew Primary" + gameObject.name);
        }
    }

    public override void SecondaryAttack(InputAction.CallbackContext context)
    {
        if(context.performed) {
            Debug.Log("Wapen Piew Secondary");
        }
    }
}
