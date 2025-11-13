using UnityEngine;

public class BodyPartHitbox : MonoBehaviour
{
    public BodyPartType bodyPartType;
    public PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    public void TakeHit(float damage)
    {
        playerStats.TakeDamage(bodyPartType, damage);
    }
}
