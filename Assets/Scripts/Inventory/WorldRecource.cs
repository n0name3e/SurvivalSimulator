using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRecource : MonoBehaviour
{
    public float health = 50f;
    public Item droppedItem;
    public int droppedItemAmount = 1;

    public void TakeDamage(float damage)
    {
        health -= damage;
        print(health);
        if (health <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Inventory.Instance.AddItem(droppedItem, droppedItemAmount);
        Destroy(gameObject);
    }
}
