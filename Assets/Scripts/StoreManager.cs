using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }

    [Header("Store Catalog")]
    [SerializeField] private List<StoreItemData> catalog = new List<StoreItemData>();

    [Header("UI Containers")]
    [SerializeField] private Transform storeContentArea;
    [SerializeField] private GameObject categoryHeaderPrefab;
    [SerializeField] private GameObject storeItemCardPrefab;

    private Dictionary<string, int> itemsOwned = new Dictionary<string, int>();
    private HashSet<string> unlockedTechIds = new HashSet<string>();
    private Dictionary<string, StoreItemUI> spawnedCards = new Dictionary<string, StoreItemUI>();

    public event Action OnStorePurchased;

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
        InitializeCatalog();
        BuildStoreUI();
    }

    void Update()
    {
        // Periodic check to update buy button interactability as energy increases
        if (EnergyManager.Instance != null && spawnedCards.Count > 0)
        {
            RefreshAllCardsUI();
        }
    }

    private void InitializeCatalog()
    {
        foreach (var item in catalog)
        {
            if (item != null && !itemsOwned.ContainsKey(item.id))
            {
                itemsOwned[item.id] = 0;
            }
        }
    }

    public void BuildStoreUI()
    {
        if (storeContentArea == null) return;

        // Clear existing children
        foreach (Transform child in storeContentArea)
        {
            Destroy(child.gameObject);
        }
        spawnedCards.Clear();

        // Categorize items
        var energyGenerators = catalog.FindAll(x => x.itemType == StoreItemType.EnergyGenerator);
        var facilityUpgrades = catalog.FindAll(x => x.itemType == StoreItemType.FacilityUpgrade);
        var livingQuarters = catalog.FindAll(x => x.itemType == StoreItemType.LivingQuarters);

        BuildCategorySection("ENERGY INFRASTRUCTURE", energyGenerators);
        BuildCategorySection("FACILITY ENHANCEMENTS", facilityUpgrades);
        BuildCategorySection("VILLAGER LIVING QUARTERS", livingQuarters);
    }

    private void BuildCategorySection(string categoryTitle, List<StoreItemData> items)
    {
        if (items == null || items.Count == 0) return;

        // Instantiate Header
        if (categoryHeaderPrefab != null && storeContentArea != null)
        {
            GameObject headerObj = Instantiate(categoryHeaderPrefab, storeContentArea);
            StoreCategoryHeaderUI headerUI = headerObj.GetComponent<StoreCategoryHeaderUI>();
            if (headerUI != null) headerUI.Setup(categoryTitle);
        }

        // Instantiate Items
        foreach (var item in items)
        {
            if (item == null) continue;

            if (storeItemCardPrefab != null && storeContentArea != null)
            {
                GameObject cardObj = Instantiate(storeItemCardPrefab, storeContentArea);
                StoreItemUI cardUI = cardObj.GetComponent<StoreItemUI>();
                if (cardUI != null)
                {
                    int owned = GetItemCount(item.id);
                    bool isUnlocked = IsItemUnlocked(item);
                    double cost = GetCurrentItemCost(item);

                    cardUI.Setup(item, owned, isUnlocked, cost, OnBuyItemClicked);
                    spawnedCards[item.id] = cardUI;
                }
            }
        }
    }

    public void RefreshAllCardsUI()
    {
        foreach (var item in catalog)
        {
            if (item != null && spawnedCards.ContainsKey(item.id))
            {
                int owned = GetItemCount(item.id);
                bool isUnlocked = IsItemUnlocked(item);
                double cost = GetCurrentItemCost(item);

                spawnedCards[item.id].RefreshUI(owned, isUnlocked, cost);
            }
        }
    }

    public void OnBuyItemClicked(StoreItemData item)
    {
        if (item == null || !IsItemUnlocked(item)) return;

        double cost = GetCurrentItemCost(item);
        if (EnergyManager.Instance != null && EnergyManager.Instance.CurrentEnergy >= cost)
        {
            // Deduct Energy
            EnergyManager.Instance.AddEnergy(-cost);

            // Increment Count
            itemsOwned[item.id] = GetItemCount(item.id) + 1;

            // Apply Bonuses
            ApplyItemEffects(item);

            // Refresh UI
            RefreshAllCardsUI();
            OnStorePurchased?.Invoke();

            Debug.Log($"[StoreManager] Purchased 1x {item.itemName}. Total owned: {itemsOwned[item.id]}");
        }
    }

    private void ApplyItemEffects(StoreItemData item)
    {
        if (item.itemType == StoreItemType.EnergyGenerator && EnergyManager.Instance != null)
        {
            EnergyManager.Instance.AddEnergyPerSecond(item.energyPerSecondBonus);
        }
    }

    public bool IsItemUnlocked(StoreItemData item)
    {
        if (item == null) return false;
        if (!item.requiresResearch) return true;
        return unlockedTechIds.Contains(item.requiredResearchId);
    }

    public void UnlockResearchTech(string techId)
    {
        if (!unlockedTechIds.Contains(techId))
        {
            unlockedTechIds.Add(techId);
            RefreshAllCardsUI();
            Debug.Log($"[StoreManager] Unlocked tech: {techId}");
        }
    }

    public int GetItemCount(string itemId)
    {
        return itemsOwned.TryGetValue(itemId, out int count) ? count : 0;
    }

    public double GetCurrentItemCost(StoreItemData item)
    {
        if (item == null) return 0;
        int count = GetItemCount(item.id);
        return item.baseCost * Math.Pow(item.costMultiplier, count);
    }

    public int GetTotalHousingCapacity()
    {
        int total = 0;
        foreach (var item in catalog)
        {
            if (item != null && item.itemType == StoreItemType.LivingQuarters)
            {
                total += item.housingCapacityBonus * GetItemCount(item.id);
            }
        }
        return total;
    }

    public int GetTotalStylingScore()
    {
        int total = 0;
        foreach (var item in catalog)
        {
            if (item != null && item.itemType == StoreItemType.LivingQuarters)
            {
                total += item.stylingScoreBonus * GetItemCount(item.id);
            }
        }
        return total;
    }
}
