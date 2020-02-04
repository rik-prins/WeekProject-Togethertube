using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    [SerializeField] private Transform portalPartner;
    [SerializeField] private bool portalEnabled = true;

    private void Start() {
        NotificationManager.OnTeleportEvent += ObjectTeleportedHandler;
    }

    private IEnumerator SetTeleportStatus() {

        yield return new WaitForSeconds( 2 );
        portalEnabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Teleportable" && portalEnabled) {
            Teleport( other.gameObject );
        }
    }

    private void Teleport(GameObject other) {
        NotificationManager.FireTeleport();



        print( transform.parent.name +" teleporting" + other.name );

        portalPartner.GetComponent<Portal>().Teleported( other );
    }

    public void Teleported(GameObject target) {

        CharacterController c = target.GetComponent<CharacterController>();

        if(c != null) {
            c.enabled = false;
        }

        target.transform.position = transform.position;

        if(c != null) {
            c.enabled = true;
        }

        print( transform.parent.name + " teleported" + target.name );
    }

    private void ObjectTeleportedHandler() {
        portalEnabled = false;
        StartCoroutine( SetTeleportStatus() );
    }

    private void OnDestroy() {
        NotificationManager.OnTeleportEvent -= ObjectTeleportedHandler;
    }
}
