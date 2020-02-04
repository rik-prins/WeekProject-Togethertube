using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject[,] grid;
    public GameObject gridTilePrefab;

    private void Awake()
    {
        grid = new GameObject[10, 10];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                GameObject prefab = null;

                prefab = gridTilePrefab;

                grid[y, x] = Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}