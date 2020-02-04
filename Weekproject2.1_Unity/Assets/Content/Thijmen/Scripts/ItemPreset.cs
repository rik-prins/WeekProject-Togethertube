using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/New Item", order = 1)]
public class ItemPreset : ScriptableObject
{
    public Item m_itemPrefab;
    public ItemID m_id;
}
