using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VillagerPanelUI : MonoBehaviour
{
    [Header("Satisfaction Meter Elements")]
    [SerializeField] private Image satisfactionFillImage;
    [SerializeField] private TextMeshProUGUI moodSmileyText;
    [SerializeField] private TextMeshProUGUI satisfactionPercentageText;
    [SerializeField] private TextMeshProUGUI outputMultiplierText;

    [Header("Recruitment & Housing Elements")]
    [SerializeField] private TextMeshProUGUI housingPopulationText;
    [SerializeField] private TextMeshProUGUI recruitmentTimerText;
    [SerializeField] private Button assignHomeButton;
    [SerializeField] private TextMeshProUGUI assignHomeButtonText;

    [Header("Advice Box")]
    [SerializeField] private TextMeshProUGUI roomForImprovementText;

    [Header("Color Gradients")]
    [SerializeField] private Color lowSatisfactionColor = new Color(0.9f, 0.25f, 0.25f, 1f); // Red
    [SerializeField] private Color normalSatisfactionColor = new Color(0.95f, 0.85f, 0.25f, 1f); // Yellow
    [SerializeField] private Color surgeSatisfactionColor = new Color(0.25f, 0.9f, 0.45f, 1f); // Green/Gold

    void Awake()
    {
        if (assignHomeButton != null)
        {
            assignHomeButton.onClick.RemoveAllListeners();
            assignHomeButton.onClick.AddListener(OnAssignButtonClicked);
        }
    }

    void OnEnable()
    {
        if (VillagerSatisfactionSystem.Instance != null)
        {
            VillagerSatisfactionSystem.Instance.OnSatisfactionChanged += RefreshUI;
        }
        RefreshUI();
    }

    void OnDisable()
    {
        if (VillagerSatisfactionSystem.Instance != null)
        {
            VillagerSatisfactionSystem.Instance.OnSatisfactionChanged -= RefreshUI;
        }
    }

    void Update()
    {
        RefreshUI();
    }

    public void OnAssignButtonClicked()
    {
        if (VillagerManager.Instance != null)
        {
            VillagerManager.Instance.AssignCandidateToHome();
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        // 1. Satisfaction UI
        if (VillagerSatisfactionSystem.Instance != null)
        {
            float satisfaction = VillagerSatisfactionSystem.Instance.CurrentSatisfaction;
            float mult = VillagerSatisfactionSystem.Instance.CurrentMultiplier;
            VillagerMoodTier tier = VillagerSatisfactionSystem.Instance.CurrentMoodTier;

            if (satisfactionFillImage != null)
            {
                satisfactionFillImage.fillAmount = Mathf.Clamp01(satisfaction / 100f);
                switch (tier)
                {
                    case VillagerMoodTier.Low: satisfactionFillImage.color = lowSatisfactionColor; break;
                    case VillagerMoodTier.Normal: satisfactionFillImage.color = normalSatisfactionColor; break;
                    case VillagerMoodTier.Surge: satisfactionFillImage.color = surgeSatisfactionColor; break;
                }
            }

            if (moodSmileyText != null) moodSmileyText.text = VillagerSatisfactionSystem.Instance.GetMoodSmiley();
            if (satisfactionPercentageText != null) satisfactionPercentageText.text = $"{satisfaction:0}% Satisfaction";

            if (outputMultiplierText != null)
            {
                string tierLabel = tier == VillagerMoodTier.Surge ? "125% SURGE BOOST!" : (tier == VillagerMoodTier.Low ? "80% Output Penalty" : "100% Normal Output");
                outputMultiplierText.text = $"Multiplier: {mult:0.##}x ({tierLabel})";
            }

            if (roomForImprovementText != null) roomForImprovementText.text = VillagerSatisfactionSystem.Instance.GetRoomForImprovementAdvice();
        }

        // 2. Recruitment & Housing UI
        if (VillagerManager.Instance != null)
        {
            int currentPop = VillagerManager.Instance.CurrentVillagerCount;
            int maxCap = VillagerManager.Instance.GetMaxHousingCapacity();
            int candidates = VillagerManager.Instance.CandidateVillagersCount;

            if (housingPopulationText != null)
            {
                housingPopulationText.text = $"Population: {currentPop} / {maxCap} Villagers Housed";
            }

            if (recruitmentTimerText != null)
            {
                if (currentPop + candidates >= maxCap)
                {
                    recruitmentTimerText.text = "Housing Full — Build Living Quarters to attract more villagers!";
                }
                else
                {
                    float timer = VillagerManager.Instance.RecruitmentTimer;
                    recruitmentTimerText.text = $"New Villager Arriving in: {timer:0}s";
                }
            }

            if (assignHomeButton != null)
            {
                bool canAssign = candidates > 0 && currentPop < maxCap;
                assignHomeButton.interactable = canAssign;

                if (assignHomeButtonText != null)
                {
                    if (candidates > 0)
                    {
                        assignHomeButtonText.text = $"Assign Candidate ({candidates} Waiting!)";
                    }
                    else
                    {
                        assignHomeButtonText.text = "No Candidates Waiting";
                    }
                }
            }
        }
    }
}
