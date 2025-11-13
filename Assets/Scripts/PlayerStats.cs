using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float MaxHealth = 100f;
    public List<BodyPart> bodyParts = new List<BodyPart>();
    public float MaxStamina = 100f;

    public float health = 100f;
    public float stamina = 100f;

    public float hunger = 100f;
    public float thirst = 100f;

    public float staminaConsumptionRate = 10f;
    public float staminaRecoveryRate = 5f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        
        health = MaxHealth;
        stamina = MaxStamina;

        AddBodyParts();
        TakeDamage(BodyPartType.RightArm, 25f);
        TakeDamage(BodyPartType.LeftLeg, 25f);
    }
    private void Update()
    {
        ReduceHunger(4f * Time.deltaTime);
        ReduceThirst(6f * Time.deltaTime);
        TakeDamage(BodyPartType.Head, 0.25f * Time.deltaTime);
        TakeDamage(BodyPartType.Torso, 0.15f * Time.deltaTime);
        if (stamina <= 0)
        {
            stamina = 0;
            playerMovement.canSprint = false;
        }
        if (stamina >= 25)
            playerMovement.canSprint = true;
        if (playerMovement.isSprinting)
        {
            stamina -= staminaConsumptionRate * Time.deltaTime;
        }
        else
        {
            stamina += staminaRecoveryRate * Time.deltaTime;
        }
        UI.instance.UpdateStaminaBar(stamina, MaxStamina);
        stamina = Mathf.Clamp(stamina, 0, MaxStamina);
    }
    public float GetBodyPartHealth(BodyPartType type)
    {
        return bodyParts.Find(x => x.bodyPartType == type).currentHealth;
    }
    public void TakeDamage(BodyPartType type, float amount)
    {
        bodyParts.Find(x => x.bodyPartType == type).TakeDamage(amount);
        UI.instance.UpdateStatsText();
    }
    public void ReduceHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger - amount, 0, 100);
    }
    public void ReduceThirst(float amount)
    {
        thirst = Mathf.Clamp(thirst - amount, 0, 100);
    }
    public void Feed(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, 100);
    }
    public void Hydrate(float amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0, 100);
    }
    private void AddBodyParts()
    {
        bodyParts.Add(new BodyPart(BodyPartType.Head, 30f, 1.5f));
        bodyParts.Add(new BodyPart(BodyPartType.Torso, 30f, 1f));
        bodyParts.Add(new BodyPart(BodyPartType.RightArm, 30f, 0.5f));
        bodyParts.Add(new BodyPart(BodyPartType.LeftArm, 30f, 0.5f));
        bodyParts.Add(new BodyPart(BodyPartType.RightLeg, 30f, 0.75f));
        bodyParts.Add(new BodyPart(BodyPartType.LeftLeg, 30f, 0.75f));
    }
}
