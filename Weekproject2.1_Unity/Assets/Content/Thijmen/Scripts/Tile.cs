using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    TileTask state;
    public enum TileTask {
        Hookable = 0,
        Normal
    }

    [SerializeField]
    private Material Normal,
        Hookable;

    [Range( 0 , 100 )]
    [SerializeField] private int hookableSpawnrate;

    void Start() {
        state = (Random.Range( 0 , hookableSpawnrate ) == 0) ? TileTask.Hookable : TileTask.Normal;
        ExecuteEnumState();
    }

    private void ExecuteEnumState() {

        if(state == TileTask.Normal) {
            gameObject.transform.localScale = new Vector3( 1 , 1 , Random.Range( 1f , 2f ) );
            transform.GetChild( 0 ).GetComponent<MeshRenderer>().material = Normal;
            GetComponentInChildren<BoxCollider>().enabled = false;
        }
        if(state == TileTask.Hookable) {
            gameObject.transform.localScale = new Vector3( 1 , 1 , 6 );
            transform.GetChild( 0 ).GetComponent<MeshRenderer>().material = Hookable;
            transform.GetChild( 0 ).tag = "Hookable";
        }
    }

    void Update() {

    }
}
