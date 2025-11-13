using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BodyPartType
{
    Head,
    Torso,
    RightArm,
    LeftArm,
    RightLeg,
    LeftLeg
}
[System.Serializable]
public class BodyPart
{
    public BodyPartType bodyPartType;
    public float maxHealth;
    public float currentHealth;

    // how much damage from this part damages overall health
    public float damageMultiplier = 0.5f;

    public BodyPart(BodyPartType type, float maxHealth, float damageMultiplier)
    {
        this.bodyPartType = type;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.damageMultiplier = damageMultiplier;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
