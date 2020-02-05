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

    private int test;

    void Start() {
        test = ySize;
        SpawnGridGameStart();
    }

    private void Update() {

        if(m_ScoreY < player.transform.position.y) {
            m_ScoreY = player.transform.position.y;

            m_ScoreCheck++;
            if(m_ScoreCheck % 5 == 0) {
                m_Score++;
                SpawnGridIngame( (int)player.transform.position.y + ySize );
            }
        }

        //if(m_ScoreY >= 20) {
        //    print( "test" + m_ScoreY );
        //    m_ScoreY = 0;
        //}
    }

    public void SpawnGridGameStart() {
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

    public void SpawnGridIngame(int _y) {
        GameObject row = new GameObject();
        row.name = "row " + test;   
        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < 1; y++) {
                GameObject newRow = Instantiate( wallTile , new Vector3( x , test + y + 0.5f , -13 ) , Quaternion.identity );
                newRow.transform.SetParent( row.transform );
            }
        }

        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < 1; y++) {
                GameObject newRow = Instantiate( wallTile , new Vector3( x , test + y + 0.5f , 13 ) , Quaternion.Euler( 0 , 180 , 0 ) );
                newRow.transform.SetParent( row.transform );
            }
        }
        test++;
    }
}
