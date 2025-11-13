using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public static ChestUI instance;
    //public ItemObject[] items = new ItemObject[20];
    public Chest openedChest;

    // list that contains all slots for items in inventory
    public GameObject[] inventorySlots = new GameObject[20];

    public GameObject chestContainer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int index = i;
            GameObject obj = inventorySlots[i];
            obj.GetComponent<Button>().onClick.AddListener(() => { LootItem(index); });
            obj.name = i.ToString();
        }
    }

    public void ShowChestUI(Chest chest)
    {
        chestContainer.SetActive(true);
        //List<Item> items = chest.items;
        openedChest = chest;
        for (int i = 0; i < openedChest.items.Length; i++)
        {
            if (i >= inventorySlots.Length)
                break;
            //Item item = openedChest.items[i];
            ItemObject itemObject = openedChest.items[i];
            itemObject.UIObject = inventorySlots[i];
            //ItemObject itemObject = new ItemObject(item, 1, inventorySlots[i]);
            itemObject.index = i;
            chest.items[i] = itemObject;
            inventorySlots[i].GetComponent<InventoryItemUI>().item = itemObject.item;
            itemObject.ShowInUI();
        }
    }
    private void LootItem(int index)
    {
        if (openedChest.items[index]?.item == null) 
            return; 

        Inventory.instance.AddItem(openedChest.items[index].item, 1); 
        RemoveItem(index);
    }
    public void RemoveItem(int index)
    {
        ItemObject existingItem = openedChest.items[index];
        if (openedChest.items[index] != null)
        {
            //existingItem.item = null;
            existingItem.RemoveFromStack(1);
            existingItem.UpdateUI();
            if (existingItem.stackQuantity <= 0)
            {
                openedChest.items[index].item = null;
            }
            FreeSlots();
        }
    }

    // called to remove all items if they are gone
    private void FreeSlots()
    {
        for (int i = 0; i < openedChest.items.Length; i++)
        {
            if (openedChest.items[i] != null && openedChest.items[i].stackQuantity <= 0)
            {
                openedChest.items[i].item = null;
                inventorySlots[i].GetComponent<InventoryItemUI>().item = null;
            }
        }
    }
    public void DisableChestContainer()
    {
        chestContainer.SetActive(false);
    }
}
