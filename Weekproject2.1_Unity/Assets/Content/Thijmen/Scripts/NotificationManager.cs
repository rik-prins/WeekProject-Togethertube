using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager
{
    public delegate void OnTeleportAction();
    public static event OnTeleportAction OnTeleportEvent;
    public static void FireTeleport() {
        OnTeleportEvent?.Invoke();
    }
}
