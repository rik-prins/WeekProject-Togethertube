using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{

    [SerializeField] private Transform m_itemContainer;
    private Dictionary<ItemID,Item> m_items;
    private const string m_itemResourcesPath = "Items/";

    private int m_equipedSlot;
    private Item m_equipedItem;
    private List<ItemID> m_inventory = new List<ItemID>();

    private void Awake()
    {
        m_items = new Dictionary<ItemID, Item>();
        ItemPreset[] m_itemsPresets = Resources.LoadAll<ItemPreset>(m_itemResourcesPath);

        foreach (ItemPreset preset in m_itemsPresets)
        {
            Item item = Instantiate(preset.m_itemPrefab, m_itemContainer.position, m_itemContainer.rotation, m_itemContainer);
            item.gameObject.SetActive(false);
            m_items.Add(preset.m_id, item);
        }

        if (m_items.ContainsKey(ItemID.knife))
        {
            m_inventory = new List<ItemID>();
            m_inventory.Add(ItemID.knife);
            m_equipedItem = m_items[ItemID.knife];
            m_equipedItem.gameObject.SetActive(true);
        }
        else
            Debug.LogWarning("No knife in recources");


        m_inventory.Add(ItemID.pistol);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            DebugInventory();
    }


    private void EquipItem(ItemID itemID)
    {
        m_equipedItem.gameObject.SetActive(false);
        m_equipedItem = m_items[itemID];
        m_equipedItem.gameObject.SetActive(true);
    }


    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        IUsable usable = m_equipedItem.usable;
        if (usable != null) usable.PrimaryAttack(context);
    }
    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        IUsable usable = m_equipedItem.usable;
        if (usable != null) usable.SecondaryAttack(context);
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        IUsable usable = m_equipedItem.usable;
        if (usable != null) usable.Reload(context);
    }
    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();

            if (value > 0)
            {
                m_equipedSlot++;
                if (m_equipedSlot >= m_inventory.Count)
                    m_equipedSlot = 0;

                EquipItem(m_inventory[m_equipedSlot]);
            }
            else if (value < 0)
            {
                m_equipedSlot--;
                if (m_equipedSlot < 0)
                    m_equipedSlot = m_inventory.Count - 1;

                EquipItem(m_inventory[m_equipedSlot]);
            }
        }
    }
    

    // DEBUG
    private void DebugInventory()
    {
        Debug.Log("ITEMS IN INVENTORY: " + m_inventory.Count);
        for (int i = 0; i < m_inventory.Count; i++)
        {
            Debug.Log(m_inventory[i]);      
        }
        Debug.Log("------------------");
    }
}
