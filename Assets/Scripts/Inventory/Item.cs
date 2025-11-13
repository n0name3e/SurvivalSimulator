using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int stackSize = 1;

    public virtual void UseItem()
    {

    }

    public virtual string GetDescription()
    {
        return itemName;
    }
}
