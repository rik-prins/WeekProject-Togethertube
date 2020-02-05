using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {
    [SerializeField] private GameObject wallTile;
    [SerializeField] private PlayerController player;
    [Range( 5 , 100 )]
    [SerializeField]
    int xSize,
        ySize;

    void Start() {
        SpawnGrid();
    }

    private void Update() {
        //if(player.height % 20 == 0) {
        //    print( player.height );
        //}
    }

    public void SpawnGrid() {
        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {
                Instantiate( wallTile , new Vector3( x , y + 0.5f , 13 ) , Quaternion.Euler( 0 , 180 , 0 ) );
            }
        }

        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {
                Instantiate( wallTile , new Vector3( x , y + 0.5f , -13 ) , Quaternion.identity );
            }
        }
    }
}
