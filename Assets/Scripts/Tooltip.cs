using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public static Tooltip instance; 

    public TMP_Text tooltipText;
    public RectTransform tooltipPanel; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameObject.SetActive(false); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        SetTooltipPosition();
    }

    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;

        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipPanel);
        SetTooltipPosition();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void SetTooltipPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector2 pivot = Vector2.zero;
        Vector3 offset = new Vector3(15, -15, 0); // Offset to the bottom-right

        // Get the tooltip's size
        float panelWidth = tooltipPanel.rect.width;
        float panelHeight = tooltipPanel.rect.height;

        // Check if it goes off the right side
        if (mousePosition.x + panelWidth + offset.x > Screen.width)
        {
            // If so, pivot to the left of the mouse
            pivot.x = 1;
            offset.x = -15;
        }

        // Check if it goes off the bottom
        if (mousePosition.y - panelHeight + offset.y < 0)
        {
            pivot.y = 1;
            offset.y = 15;
        }

        tooltipPanel.pivot = pivot;
        tooltipPanel.position = mousePosition + offset;
    }
}