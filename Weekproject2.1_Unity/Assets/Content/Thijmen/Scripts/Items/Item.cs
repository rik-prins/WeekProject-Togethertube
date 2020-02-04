using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public IUsable usable;

    private void Awake()
    {
        usable = GetComponent<IUsable>();
    }
}
