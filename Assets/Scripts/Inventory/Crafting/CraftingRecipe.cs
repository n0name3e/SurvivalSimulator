using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Scriptable Objects/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftingIngredient> ingredients;
    public Item craftedItem;
    public int craftedItemQuantity = 1;
}
