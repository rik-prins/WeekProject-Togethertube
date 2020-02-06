using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileTask state;

    public enum TileTask
    {
        Hookable = 0,
        Normal
    }

    [SerializeField]
    private Material Normal,
        Hookable;

    [Range(0, 100)]
    [SerializeField] private int hookableSpawnrate;

    private void Start()
    {
        state = (Random.Range(0, hookableSpawnrate) == 0) ? TileTask.Hookable : TileTask.Normal;
        ExecuteEnumState();
    }

    private void ExecuteEnumState()
    {
        if (state == TileTask.Normal)
        {
            //transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.localScale = new Vector3(1, 1, Random.Range(1f, 2f));
            int random = Random.Range(0, 2);

            if (random == 2)
            {
                transform.rotation = new Quaternion(0, 90, 0, 0);
            }
            if (random == 1)
            {
                transform.rotation = new Quaternion(0, 0, 180, 0);
            }

            transform.GetChild(0).GetComponent<MeshRenderer>().material = Normal;
            GetComponentInChildren<BoxCollider>().enabled = false;
        }
        if (state == TileTask.Hookable)
        {
            //transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.localScale = new Vector3(1, 1, 6);
            transform.GetChild(1).GetComponent<MeshRenderer>().material = Hookable;
            transform.GetChild(1).tag = "Hookable";
        }
    }

    private void Update()
    {
    }
}