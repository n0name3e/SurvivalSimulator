using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;

    public List<ItemObject> droppedItems;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        // Drop items
        Chest chest = Instantiate(Resources.Load<Chest>("Chest"), transform.position, Quaternion.identity);
        chest.items = droppedItems.ToArray();
        Destroy(gameObject);
    }
}
