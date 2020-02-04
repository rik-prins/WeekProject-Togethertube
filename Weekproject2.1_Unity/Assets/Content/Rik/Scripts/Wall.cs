using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void Start()
    {
        float cubeSize = Random.Range(1, 6);
        int randomPick = Random.Range(0, 10);

        if (randomPick == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, cubeSize);
        }
    }
}