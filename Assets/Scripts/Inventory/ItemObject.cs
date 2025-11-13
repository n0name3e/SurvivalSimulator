using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemObject
{
    public Item item;
    public int stackQuantity = 1;
    [HideInInspector] public int index;

    // UI
    [HideInInspector] public GameObject UIObject;
    [HideInInspector] public Image iconImage;
    [HideInInspector] public TMP_Text quantityText;

    public ItemObject(Item item, int quantity, GameObject UIObject)
    {
        this.item = item;
        stackQuantity = quantity;
        this.UIObject = UIObject;

        ShowInUI();
    }
    public void ShowInUI()
    {
        iconImage = UIObject.GetComponent<Image>();
        quantityText = UIObject.GetComponentInChildren<TMP_Text>();
        UpdateUI();
    }
    public void AddToStack(int quantity)
    {
        stackQuantity += quantity;
        if (stackQuantity > item.stackSize)
        {
            stackQuantity = item.stackSize;

            // todo add new item when stack is full i guess
            Debug.Log("Stack is full: " + item.itemName);
        }
        UpdateUI();
    }
    public void RemoveFromStack(int quantity)
    {
        stackQuantity -= quantity;
        if (stackQuantity <= 0)
        {
            stackQuantity = 0;
            item = null;
            UIObject.GetComponent<InventoryItemUI>().item = null;
        }
        UpdateUI();
    }
    public bool IsEmpty()
    {
        return item == null || stackQuantity <= 0;
    }
    public void UpdateUI()
    {
        Debug.Log("Updating UI for item: " + item?.itemName + " with quantity: " + stackQuantity);
        if (item == null)
        {
            iconImage.sprite = null;
            quantityText.text = "";
            return;
        }
        iconImage.sprite = item.icon;
        quantityText.text = stackQuantity.ToString();
    }
    public virtual void UseItem()
    {
        Debug.Log("item is used" + item?.itemName);
    }
}
