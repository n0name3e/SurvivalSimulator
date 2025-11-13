using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    // No need for a public Tooltip variable, we use the singleton

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ShowItemTooltip();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.HideTooltip();
    }

    public void ShowItemTooltip()
    {
        Tooltip.instance.ShowTooltip(item.GetDescription());
    }
}
