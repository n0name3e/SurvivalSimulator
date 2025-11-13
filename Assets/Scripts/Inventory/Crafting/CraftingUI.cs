using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI instance;

    public List<CraftingRecipe> allRecipes; // Assign all your recipe assets here
    public GameObject recipeSlotPrefab; // Your prefab
    public Transform recipeContainer; // The "Content" object of your ScrollView
    public GameObject craftingWindow;

    public List<RecipeSlotUI> recipeSlots = new List<RecipeSlotUI>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PopulateRecipeList();
    }

    private void PopulateRecipeList()
    {
        foreach (CraftingRecipe recipe in allRecipes)
        {
            GameObject slotGO = Instantiate(recipeSlotPrefab, recipeContainer);
            slotGO.GetComponent<RecipeSlotUI>().Setup(recipe);
            recipeSlots.Add(slotGO.GetComponent<RecipeSlotUI>());
        }
    }
    public void UpdateUI()
    {
        foreach (RecipeSlotUI slot in recipeSlots)
        {
            slot.UpdateUI();
        }
    }

    public string ShowRecipeDescription(CraftingRecipe recipe)
    {

        string ingredientsStr = "Requires:\n";
        foreach (CraftingIngredient ingredient in recipe.ingredients)
        {
            ingredientsStr += $"- {ingredient.item.name} " +
                $"({ingredient.quantity}/{Inventory.instance.GetItemCount(ingredient.item)})\n";
        }
        //ingredientsText.text = ingredientsStr;

        // Check if we can craft it to enable/disable the button

        return ingredientsStr;
    }

    public void EnableCraftingWindow(bool enable)
    {
        craftingWindow.SetActive(enable);
    }
    /*public void DisableCraftingWindow()
    {
        craftingWindow.SetActive(false);
    }*/
}