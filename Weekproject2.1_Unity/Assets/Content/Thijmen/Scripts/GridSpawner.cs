using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {
    [SerializeField] private GameObject wallTile;
    [SerializeField] private PlayerController player;
    [SerializeField] private float m_ScoreY, m_ScoreCheck;
    [SerializeField] private int m_Score;
    [Range( 5 , 100 )]
    [SerializeField]
    private int xSize,
        ySize;

    void Start() {
        SpawnGrid();
    }

    private void Update() {

        if(m_ScoreY < player.transform.position.y) {
            m_ScoreY = player.transform.position.y;

            m_ScoreCheck++;
            if(m_ScoreCheck % 20 == 0) {
                m_Score++;
                print( "Score" );
            }
        }
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
