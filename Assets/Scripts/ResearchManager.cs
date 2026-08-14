using System;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public static ResearchManager Instance { get; private set; }

    [Header("Research Catalog")]
    [SerializeField] private List<ResearchItemData> catalog = new List<ResearchItemData>();

    [Header("UI References")]
    [SerializeField] private Transform researchContentArea;
    [SerializeField] private GameObject researchCardPrefab;

    private HashSet<string> completedResearchIds = new HashSet<string>();
    private Dictionary<string, ResearchItemUI> spawnedCards = new Dictionary<string, ResearchItemUI>();

    public event Action OnResearchCompleted;

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
        BuildResearchUI();
    }

    void Update()
    {
        if (EnergyManager.Instance != null && spawnedCards.Count > 0)
        {
            RefreshAllCardsUI();
        }
    }

    public void BuildResearchUI()
    {
        if (researchContentArea == null || researchCardPrefab == null) return;

        foreach (Transform child in researchContentArea)
        {
            Destroy(child.gameObject);
        }
        spawnedCards.Clear();

        foreach (var item in catalog)
        {
            if (item == null) continue;

            GameObject cardObj = Instantiate(researchCardPrefab, researchContentArea);
            ResearchItemUI cardUI = cardObj.GetComponent<ResearchItemUI>();
            if (cardUI != null)
            {
                bool isResearched = IsResearchCompleted(item.researchId);
                cardUI.Setup(item, isResearched, OnResearchButtonClicked);
                spawnedCards[item.researchId] = cardUI;
            }
        }
    }

    public void RefreshAllCardsUI()
    {
        foreach (var item in catalog)
        {
            if (item != null && spawnedCards.ContainsKey(item.researchId))
            {
                bool isResearched = IsResearchCompleted(item.researchId);
                spawnedCards[item.researchId].RefreshUI(isResearched);
            }
        }
    }

    public void OnResearchButtonClicked(ResearchItemData item)
    {
        if (item == null || IsResearchCompleted(item.researchId)) return;

        if (EnergyManager.Instance != null && EnergyManager.Instance.CurrentEnergy >= item.energyCost)
        {
            // Deduct Energy
            EnergyManager.Instance.AddEnergy(-item.energyCost);

            // Mark completed
            completedResearchIds.Add(item.researchId);

            // Unlock Store Item / Tech
            if (StoreManager.Instance != null && !string.IsNullOrEmpty(item.unlocksTechId))
            {
                StoreManager.Instance.UnlockResearchTech(item.unlocksTechId);
            }

            RefreshAllCardsUI();
            OnResearchCompleted?.Invoke();

            Debug.Log($"[ResearchManager] Completed research '{item.researchName}'! Unlocked tech '{item.unlocksTechId}'");
        }
    }

    public bool IsResearchCompleted(string researchId)
    {
        return completedResearchIds.Contains(researchId);
    }
}
