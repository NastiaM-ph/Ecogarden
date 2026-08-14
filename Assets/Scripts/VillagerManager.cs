using System;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public static VillagerManager Instance { get; private set; }

    [Header("Villager & Housing Stats")]
    [SerializeField] private int baseVillagerCount = 2;
    public int CurrentVillagerCount { get; private set; } = 2;
    public int CandidateVillagersCount { get; private set; } = 0;

    [Header("Recruitment Timer Settings")]
    [SerializeField] private float baseRecruitmentInterval = 20f;
    public float RecruitmentTimer { get; private set; } = 0f;

    public event Action OnVillagerCountChanged;
    public event Action OnCandidateArrived;

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
        CurrentVillagerCount = baseVillagerCount;
        RecruitmentTimer = GetEffectiveRecruitmentInterval();
    }

    void Update()
    {
        UpdateRecruitmentTimer();
    }

    public int GetMaxHousingCapacity()
    {
        int bonusHousing = StoreManager.Instance != null ? StoreManager.Instance.GetTotalHousingCapacity() : 0;
        return baseVillagerCount + bonusHousing;
    }

    public float GetEffectiveRecruitmentInterval()
    {
        int styleScore = StoreManager.Instance != null ? StoreManager.Instance.GetTotalStylingScore() : 0;
        // Sped up by Garden Styling Score (minimum 5s interval)
        float speedMultiplier = 1f + (styleScore * 0.02f);
        return Mathf.Max(5f, baseRecruitmentInterval / speedMultiplier);
    }

    private void UpdateRecruitmentTimer()
    {
        int maxCapacity = GetMaxHousingCapacity();
        int totalTargeting = CurrentVillagerCount + CandidateVillagersCount;

        if (totalTargeting < maxCapacity)
        {
            RecruitmentTimer -= Time.deltaTime;
            if (RecruitmentTimer <= 0f)
            {
                CandidateVillagersCount++;
                RecruitmentTimer = GetEffectiveRecruitmentInterval();
                OnCandidateArrived?.Invoke();
                Debug.Log($"[VillagerManager] A candidate villager has arrived! Waiting candidates: {CandidateVillagersCount}");
            }
        }
        else
        {
            // Reset timer while full
            RecruitmentTimer = GetEffectiveRecruitmentInterval();
        }
    }

    public bool AssignCandidateToHome()
    {
        if (CandidateVillagersCount > 0 && CurrentVillagerCount < GetMaxHousingCapacity())
        {
            CandidateVillagersCount--;
            CurrentVillagerCount++;
            OnVillagerCountChanged?.Invoke();
            Debug.Log($"[VillagerManager] Candidate assigned to home! Current Population: {CurrentVillagerCount}/{GetMaxHousingCapacity()}");
            return true;
        }
        return false;
    }
}
