using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public List<ItemObject> startingItems;

    public PlayerStats player;
    
    public ItemObject[] items = new ItemObject[20];
    //public List<ItemObject> items = new List<ItemObject>();
    // list that contains all slots for items in inventory
    public GameObject[] inventorySlots = new GameObject[20];
    //public List<GameObject> itemGameObjects = new List<GameObject>();
    public GameObject toolSlotUI;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        foreach (ItemObject item in startingItems)
        {
            AddItem(item.item, item.stackQuantity);
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int index = i;
            GameObject obj = inventorySlots[i];
            obj.GetComponent<Button>().onClick.AddListener(() => UseItem(index));
            obj.name = i.ToString();
        }
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
    }

    public void AddItem(Item item, int quantity)
    {
        if (FindItem(item) is ItemObject existingItem && existingItem != null)
        {
            existingItem.AddToStack(quantity);
            CraftingUI.instance.UpdateUI();
            return;
        }

        int index = FindFreeSlot();
        if (index == -1)
            return;
        ItemObject itemObject = new ItemObject(item, quantity, inventorySlots[index]);
        itemObject.index = index;
        items[index] = itemObject;
        inventorySlots[index].GetComponent<InventoryItemUI>().item = item;
        CraftingUI.instance.UpdateUI();
    }
    public void RemoveItem(ItemObject item, int quantity = 1)
    {
        int quantityLeft = quantity;

        for (int i = items.Length - 1; i >= 0; i--)
        {
            if (items[i] == item)
            {
                int quantityInSlot = items[i].stackQuantity;

                if (quantityInSlot < quantityLeft)
                {
                    items[i].RemoveFromStack(quantityInSlot);
                    quantityLeft -= quantityInSlot;
                }
                else
                {
                    items[i].RemoveFromStack(quantityLeft);
                }
                if (quantityLeft <= 0)
                    break;
            }
        }
        // item.RemoveFromStack(quantity);
        FreeSlots();
        CraftingUI.instance.UpdateUI();

        /*if (FindItem(item) is ItemObject existingItem && existingItem != null)
        {
            existingItem.RemoveFromStack(1);
            FreeSlots();
        }*/
    }
    /*
    public void EquipTool(Tool tool)
    {
        if (equippedTool != null)
        {
            AddItem(equippedTool, 1);
            Destroy(toolObject);
        }
        equippedTool = tool;
        toolObject = Instantiate(tool.toolPrefab, handTransform);

        new ItemObject(tool, 1, toolSlotUI);
    }
    public void SwingWithTool()
    {
        toolObject.GetComponent<Animator>().Play("Swing");
    }

    // called from HandsAnimatorHandler 
    public void HitWithTool()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f))
        {
            if (hit.collider.GetComponent<WorldRecource>() != null)
            {
                hit.collider.GetComponent<WorldRecource>().TakeDamage(25);
            }
        }
        print("hit with tool" );
    }*/
    public ItemObject FindItem(Item item)
    {
        foreach (ItemObject itemObject in items)
        {
            if (itemObject != null && itemObject.item == item)
            {
                return itemObject;
            }
        }
        return null;
    }
    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (ItemObject itemObject in items)
        {
            if (itemObject.item != null && itemObject.item == item)
            {
                count += itemObject.stackQuantity;
            }
        }
        return count;
    }
    public int FindFreeSlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == null)
            {
                return i;
            }
        }
        print("no slot found");
        return -1;
    }
    public void UseItem(int index)
    {
        if (items[index] != null && items[index].item != null)
        {
            //items[index]?.UseItem();
            if (items[index].item is Consumable consumable)
            {
                consumable.Use(player);
            }
            if (items[index].item is Tool tool)
            {
                PlayerToolController.Instance.EquipTool(tool);

                new ItemObject(tool, 1, toolSlotUI);
            }
            RemoveItem(items[index], 1);
            UI.instance.UpdateStatsText();
        }
    }
    private void FreeSlots()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item != null && items[i].stackQuantity <= 0)
            {
                print("empty");
                items[i].item = null;

                //inventorySlots[i].GetComponent<InventoryItemUI>().item = null;
            }
        }
    }
}
