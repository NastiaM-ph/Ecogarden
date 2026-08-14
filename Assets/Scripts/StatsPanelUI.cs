using UnityEngine;
using TMPro;

public class StatsPanelUI : MonoBehaviour
{
    [Header("Production Stats Elements")]
    [SerializeField] private TextMeshProUGUI currentEnergyText;
    [SerializeField] private TextMeshProUGUI energyPerSecondText;
    [SerializeField] private TextMeshProUGUI energyPerClickText;
    [SerializeField] private TextMeshProUGUI totalLifetimeEnergyText;

    [Header("Villager & Housing Stats Elements")]
    [SerializeField] private TextMeshProUGUI villagerPopulationText;
    [SerializeField] private TextMeshProUGUI candidateVillagersText;
    [SerializeField] private TextMeshProUGUI gardenStylingScoreText;
    [SerializeField] private TextMeshProUGUI satisfactionMultiplierText;

    void OnEnable()
    {
        RefreshUI();
    }

    void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // 1. Production Metrics
        if (EnergyManager.Instance != null)
        {
            if (currentEnergyText != null) currentEnergyText.text = FormatNumber(EnergyManager.Instance.CurrentEnergy);
            if (energyPerSecondText != null) energyPerSecondText.text = $"{FormatNumber(EnergyManager.Instance.EnergyPerSecond)} / sec";
            if (energyPerClickText != null) energyPerClickText.text = $"{FormatNumber(EnergyManager.Instance.EnergyPerClick)} / click";
            if (totalLifetimeEnergyText != null) totalLifetimeEnergyText.text = FormatNumber(EnergyManager.Instance.TotalLifetimeEnergy);
        }

        // 2. Villager & Housing Metrics
        if (VillagerManager.Instance != null)
        {
            int current = VillagerManager.Instance.CurrentVillagerCount;
            int maxCap = VillagerManager.Instance.GetMaxHousingCapacity();
            if (villagerPopulationText != null) villagerPopulationText.text = $"{current} / {maxCap}";

            int candidates = VillagerManager.Instance.CandidateVillagersCount;
            if (candidateVillagersText != null) candidateVillagersText.text = candidates > 0 ? $"{candidates} Waiting for House!" : "None";
        }

        if (StoreManager.Instance != null && gardenStylingScoreText != null)
        {
            gardenStylingScoreText.text = $"{StoreManager.Instance.GetTotalStylingScore()} Points";
        }

        if (VillagerSatisfactionSystem.Instance != null && satisfactionMultiplierText != null)
        {
            float mult = VillagerSatisfactionSystem.Instance.CurrentMultiplier;
            string moodSmiley = VillagerSatisfactionSystem.Instance.GetMoodSmiley();
            satisfactionMultiplierText.text = $"{moodSmiley} {mult:0.##}x Multiplier";
        }
    }

    private string FormatNumber(double value)
    {
        if (value >= 1000000) return $"{value / 1000000:0.##}M";
        if (value >= 1000) return $"{value / 1000:0.##}K";
        return $"{value:0}";
    }
}
