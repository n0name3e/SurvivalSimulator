using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public PlayerStats playerStats;

    [SerializeField] private Image hpBar;
    [SerializeField] private Image staminaBar;

    // object with all elements in inventory panel (hunger, thirst, inventory and maybe more in the future)
    [SerializeField] private GameObject inventoryContainer;

    [SerializeField] private TMP_Text overallHealthText;
    [SerializeField] private TMP_Text hungerText;
    [SerializeField] private TMP_Text thirstText;

    [Header("Body Parts")]
    [SerializeField] private TMP_Text headText;
    [SerializeField] private TMP_Text torsoText;
    [SerializeField] private TMP_Text rightArmText;
    [SerializeField] private TMP_Text leftArmText;
    [SerializeField] private TMP_Text rightLegText;
    [SerializeField] private TMP_Text leftLegText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        hpBar.fillAmount = currentHealth / maxHealth;
    }
    public void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
    }

    // in inventory
    public void UpdateStatsText()
    {
        overallHealthText.text = $"{playerStats.health:0}/100";
        hungerText.text = $"{playerStats.hunger:0}/100";
        thirstText.text = $"{playerStats.thirst:0}/100";

        UpdateBodyPartsHealth();
    }
    private void UpdateBodyPartsHealth()
    {
        headText.text = $"{playerStats.GetBodyPartHealth(BodyPartType.Head).ToString("0")}/30";
        torsoText.text = $"{playerStats.GetBodyPartHealth(BodyPartType.Torso).ToString("0")}/30";
        rightArmText.text = $"{playerStats.GetBodyPartHealth(BodyPartType.RightArm).ToString("0")}/30";
        leftArmText.text = $"{playerStats.GetBodyPartHealth(BodyPartType.LeftArm).ToString("0")}/30";
        rightLegText.text = $"{playerStats.GetBodyPartHealth(BodyPartType.RightLeg).ToString("0")}/30";
        leftLegText.text = $"{playerStats.GetBodyPartHealth(BodyPartType.LeftLeg).ToString("0")}/30";
    }

    // or hide
    public void ShowInventoryPanel(bool enable)
    {
        inventoryContainer.SetActive(enable);
        UpdateStatsText();
    }
}
