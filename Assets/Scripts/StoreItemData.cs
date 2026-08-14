using UnityEngine;

public enum StoreItemType
{
    EnergyGenerator,
    FacilityUpgrade,
    LivingQuarters
}

[CreateAssetMenu(fileName = "NewStoreItem", menuName = "Ecogarden/Store Item")]
public class StoreItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string id;
    public string itemName;
    [TextArea] public string description;
    public StoreItemType itemType;
    public Sprite itemIcon;

    [Header("Pricing & Scaling")]
    public double baseCost = 10;
    public float costMultiplier = 1.15f;

    [Header("Stats Bonuses")]
    [Tooltip("Used if ItemType == EnergyGenerator")]
    public double energyPerSecondBonus = 0;

    [Tooltip("Used if ItemType == FacilityUpgrade (Percentage e.g. 15 for +15%)")]
    public float energyPercentBonus = 0f;

    [Tooltip("Used if ItemType == LivingQuarters")]
    public int housingCapacityBonus = 0;

    [Tooltip("Used if ItemType == LivingQuarters")]
    public int stylingScoreBonus = 0;

    [Header("Research Requirements")]
    public bool requiresResearch = false;
    public string requiredResearchId = "";
    public string requiredResearchName = "Advanced Tech";
}
