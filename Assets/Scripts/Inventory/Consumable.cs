using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable Objects/Consumable")]
public class Consumable : Item
{
    public float food;
    public float thirst;

    public void Use(PlayerStats user)
    {
        user.Feed(food);
        user.Hydrate(thirst);
    }
}
