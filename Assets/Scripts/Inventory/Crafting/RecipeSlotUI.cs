using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlotUI : MonoBehaviour
{
    public CraftingRecipe recipe;
    public TMP_Text text; // contains name and recipe
    public Button button;
    public Image icon;

    public void Setup(CraftingRecipe recipe)
    {
        this.recipe = recipe;

        UpdateUI();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSlotClicked);
    }

    public void UpdateUI()
    {
        icon.sprite = recipe.craftedItem.icon;
        text.text = CraftingUI.instance.ShowRecipeDescription(recipe);


    }

    void OnSlotClicked()
    {
        // Tell the main CraftingUI that this recipe was selected
        CraftingManager.instance.Craft(recipe);
    }
}