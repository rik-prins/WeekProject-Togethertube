using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : Item, IUsable
{
    public virtual void PrimaryAttack(InputAction.CallbackContext context){}

    public virtual void Reload(InputAction.CallbackContext context){}

    public virtual void SecondaryAttack(InputAction.CallbackContext context){}

}
