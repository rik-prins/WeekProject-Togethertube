using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IUsable
{
    void PrimaryAttack(InputAction.CallbackContext context);
    void SecondaryAttack(InputAction.CallbackContext context);
    void Reload(InputAction.CallbackContext context);
}
