using System;
using UnityEngine;

public enum VillagerMoodTier
{
    Low,      // < 40%: 80% Output (0.8x)
    Normal,   // 40 - 80%: 100% Output (1.0x)
    Surge     // > 80%: 125% Output Surge (1.25x)
}

public class VillagerSatisfactionSystem : MonoBehaviour
{
    public static VillagerSatisfactionSystem Instance { get; private set; }

    [Header("Satisfaction Values")]
    [Range(0, 100)]
    [SerializeField] private float currentSatisfaction = 50f;

    public float CurrentSatisfaction => currentSatisfaction;
    public VillagerMoodTier CurrentMoodTier { get; private set; } = VillagerMoodTier.Normal;
    public float CurrentMultiplier { get; private set; } = 1.0f;

    public event Action OnSatisfactionChanged;

    private float lastClickInteractionTime = -100f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        RecalculateSatisfaction();
    }

    void Update()
    {
        RecalculateSatisfaction();
    }

    public void RegisterVillagerClick()
    {
        lastClickInteractionTime = Time.time;
        // Small temporary boost for interacting with villagers
        currentSatisfaction = Mathf.Clamp(currentSatisfaction + 2f, 0f, 100f);
        RecalculateSatisfaction();
    }

    public void RecalculateSatisfaction()
    {
        float styleScore = 0;
        if (StoreManager.Instance != null)
        {
            styleScore = StoreManager.Instance.GetTotalStylingScore();
        }

        // Base satisfaction derived from Garden Styling Score + recent interaction
        float target = 35f + Mathf.Min(styleScore * 0.8f, 50f);

        // Interaction decay / bonus
        if (Time.time - lastClickInteractionTime < 15f)
        {
            target += 15f;
        }

        // Smooth interpolation
        currentSatisfaction = Mathf.Lerp(currentSatisfaction, Mathf.Clamp(target, 0f, 100f), Time.deltaTime * 2f);

        // Determine Mood Tier & Multiplier
        if (currentSatisfaction < 40f)
        {
            CurrentMoodTier = VillagerMoodTier.Low;
            CurrentMultiplier = 0.8f;
        }
        else if (currentSatisfaction <= 80f)
        {
            CurrentMoodTier = VillagerMoodTier.Normal;
            CurrentMultiplier = 1.0f;
        }
        else
        {
            CurrentMoodTier = VillagerMoodTier.Surge;
            CurrentMultiplier = 1.25f;
        }

        OnSatisfactionChanged?.Invoke();
    }

    public string GetMoodSmiley()
    {
        switch (CurrentMoodTier)
        {
            case VillagerMoodTier.Low: return "😡";
            case VillagerMoodTier.Normal: return "😐";
            case VillagerMoodTier.Surge: return "🤩";
            default: return "😐";
        }
    }

    public string GetRoomForImprovementAdvice()
    {
        int styleScore = StoreManager.Instance != null ? StoreManager.Instance.GetTotalStylingScore() : 0;

        if (currentSatisfaction < 40f)
        {
            return "Room for Improvement: Build Living Quarters (Mud, Treetop, or Mushroom houses) to raise Garden Styling Score!";
        }
        else if (currentSatisfaction <= 80f)
        {
            if (styleScore < 25)
            {
                return "Room for Improvement: Upgrade to Treetop Housing (+25 Style) or Mushroom Houses (+75 Style) to reach a 125% Output Surge!";
            }
            return "Room for Improvement: Tap wandering villagers frequently to boost morale toward a 125% Output Surge!";
        }
        else
        {
            return "Garden Status: Peak Morale! Villagers are in a 125% Output Surge! Keep building high-tier housing to sustain it.";
        }
    }
}
