using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Scriptable Objects/Tool")]
public class Tool : Item
{
    public GameObject toolPrefab; // with hands!

    public float damage = 10f;
    public float attackSpeed = 1f;
}
