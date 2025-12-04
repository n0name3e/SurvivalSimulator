using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (CraftingIngredient ingredient in recipe.ingredients)
        {
            // Check if the player has enough of this ingredient
            if (Inventory.Instance.GetItemCount(ingredient.item) < ingredient.quantity)
            {
                return false; // Not enough
            }
        }
        return true; // Has all ingredients
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("Cannot craft " + recipe.craftedItem.name);
            return;
        }

        // 1. Consume the ingredients
        foreach (CraftingIngredient ingredient in recipe.ingredients)
        {
            Inventory.Instance.RemoveItem(Inventory.Instance.FindItem(ingredient.item), ingredient.quantity);
        }

        // 2. Give the crafted item
        Inventory.Instance.AddItem(recipe.craftedItem, recipe.craftedItemQuantity);

        Debug.Log("Crafted " + recipe.craftedItem.name);

        CraftingUI.instance.UpdateUI();
    }
}