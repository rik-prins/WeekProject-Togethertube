using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {
    [SerializeField] private GameObject wallTile;
    [SerializeField] private PlayerController player;
    [SerializeField] private float m_ScoreY, m_ScoreCheck;
    [SerializeField] private int m_Score;

    [Range( 5 , 500 )]
    [SerializeField]
    private int xSize,
        ySize;

    private GameObject newRow;
    private int rowIndexL = 0;
    private int rowIndexR = 0;

    private int rowIndexL22 = 0;
    private int rowIndexR22 = 0;
    private void Start() {
        SpawnGridGameStart();
    }

    private void Update() {
        if(m_ScoreY <= player.transform.position.y && player.transform.position.y > 150 ) {
            m_ScoreY = player.transform.position.y;

            m_ScoreCheck++;
            if(m_ScoreCheck % 10 == 0) {
                m_Score++;
                rowIndexR++;
                rowIndexL++;
                DestroyRow();
            }
        }

        if(rowIndexL >= ySize) {
            rowIndexL = 0;
            rowIndexR = 0;
        }
    }

    public void SpawnGridGameStart() {
        for(int y = 0; y < ySize; y++) {
            GameObject rowLeft = new GameObject();
            rowLeft.name = "rowLeft " + rowIndexL22;
            rowIndexL22++;
            for(int x = 0; x < xSize; x++) {
                newRow = Instantiate( wallTile , new Vector3( x , y + 0.5f , 13 ) , Quaternion.Euler( 0 , 180 , 0 ) );
                newRow.transform.SetParent( rowLeft.transform );
            }
        }

        for(int y = 0; y < ySize; y++) {
            GameObject rowRight = new GameObject();
            rowRight.name = "rowRight " + rowIndexR22;
            rowIndexR22++;
            for(int x = 0; x < xSize; x++) {
                newRow = Instantiate( wallTile , new Vector3( x , y + 0.5f , -13 ) , Quaternion.identity );
                newRow.transform.SetParent( rowRight.transform );
            }
        }
    }
    
    private void DestroyRow() {
        GameObject movableObjL = GameObject.Find( "rowLeft " + (rowIndexL - 1) );
        movableObjL.transform.position = new Vector3( movableObjL.transform.position.x , movableObjL.transform.position.y + ySize , movableObjL.transform.position.z );

        GameObject movableObjR = GameObject.Find( "rowRight " + (rowIndexR - 1) );
        movableObjR.transform.position = new Vector3( movableObjR.transform.position.x , movableObjR.transform.position.y + ySize , movableObjR.transform.position.z );
    }
}