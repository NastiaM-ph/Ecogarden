using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StoreItemUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Common Icons & Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite questionMarkSprite;

    [Header("Unlocked Mode UI")]
    [SerializeField] private GameObject unlockedContainer;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI statBonusText;
    [SerializeField] private TextMeshProUGUI countOwnedText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    [Header("Locked (?) Mode UI")]
    [SerializeField] private GameObject lockedContainer;
    [SerializeField] private TextMeshProUGUI lockedTitleText;
    [SerializeField] private TextMeshProUGUI lockedCostText;
    [SerializeField] private TextMeshProUGUI lockedRequirementText;

    [Header("Click / Tap Info Popup (Mobile Friendly)")]
    [SerializeField] private GameObject hoverTooltipPanel;
    [SerializeField] private TextMeshProUGUI hoverTooltipText;

    private StoreItemData itemData;
    private bool isCurrentlyUnlocked;
    private double itemCurrentCost;
    private System.Action<StoreItemData> onBuyClickedCallback;

    public void Setup(StoreItemData data, int countOwned, bool isUnlocked, double currentCost, System.Action<StoreItemData> onBuyClicked)
    {
        this.itemData = data;
        this.onBuyClickedCallback = onBuyClicked;

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => onBuyClickedCallback?.Invoke(itemData));
        }

        RefreshUI(countOwned, isUnlocked, currentCost);
    }

    public void RefreshUI(int countOwned, bool isUnlocked, double currentCost)
    {
        if (itemData == null) return;

        this.isCurrentlyUnlocked = isUnlocked;
        this.itemCurrentCost = currentCost;

        if (hoverTooltipPanel != null) hoverTooltipPanel.SetActive(false);

        if (!isUnlocked)
        {
            // --- LOCKED (?) MODE ---
            if (unlockedContainer != null) unlockedContainer.SetActive(false);
            if (lockedContainer != null) lockedContainer.SetActive(true);

            // Icon replaced by Question Mark '?'
            if (iconImage != null)
            {
                if (questionMarkSprite != null)
                {
                    iconImage.sprite = questionMarkSprite;
                    iconImage.enabled = true;
                }
            }

            // Locked mode displays Name & Base Cost
            if (lockedTitleText != null) lockedTitleText.text = itemData.itemName;
            if (lockedCostText != null) lockedCostText.text = $"Cost: {FormatNumber(currentCost)} Energy";
            if (lockedRequirementText != null) lockedRequirementText.text = $"Tap for Info | Requires: {itemData.requiredResearchName}";
            if (buyButton != null) buyButton.interactable = false;
        }
        else
        {
            // --- UNLOCKED MODE ---
            if (unlockedContainer != null) unlockedContainer.SetActive(true);
            if (lockedContainer != null) lockedContainer.SetActive(false);

            // Custom Item Icon
            if (iconImage != null)
            {
                if (itemData.itemIcon != null)
                {
                    iconImage.sprite = itemData.itemIcon;
                    iconImage.enabled = true;
                }
            }

            if (titleText != null) titleText.text = itemData.itemName;
            if (descriptionText != null) descriptionText.text = itemData.description;
            if (countOwnedText != null) countOwnedText.text = $"x{countOwned}";
            if (costText != null) costText.text = FormatNumber(currentCost);

            // Format Typed Stat Metric
            if (statBonusText != null)
            {
                switch (itemData.itemType)
                {
                    case StoreItemType.LivingQuarters:
                        statBonusText.text = $"+{itemData.housingCapacityBonus} Housing  |  +{itemData.stylingScoreBonus} Style";
                        break;
                    case StoreItemType.EnergyGenerator:
                        statBonusText.text = $"+{FormatNumber(itemData.energyPerSecondBonus)} Energy/sec";
                        break;
                    case StoreItemType.FacilityUpgrade:
                        statBonusText.text = $"+{itemData.energyPercentBonus}% Output Boost";
                        break;
                }
            }

            // Enable Buy Button if player can afford it
            if (EnergyManager.Instance != null && buyButton != null)
            {
                buyButton.interactable = EnergyManager.Instance.CurrentEnergy >= currentCost;
            }
        }
    }

    // Click / Tap Handler (Mobile-Friendly Toggle)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null) return;

        if (!isCurrentlyUnlocked && hoverTooltipPanel != null && hoverTooltipText != null)
        {
            // Toggle Info Modal/Tooltip on click/tap
            bool currentActive = hoverTooltipPanel.activeSelf;
            hoverTooltipPanel.SetActive(!currentActive);

            if (!currentActive)
            {
                hoverTooltipText.text = $"<b>{itemData.itemName}</b>\nCost: {FormatNumber(itemCurrentCost)} Energy\n<i>Research to Unlock: {itemData.requiredResearchName}</i>";
            }
        }
    }

    public void CloseInfoPopup()
    {
        if (hoverTooltipPanel != null)
        {
            hoverTooltipPanel.SetActive(false);
        }
    }

    private string FormatNumber(double value)
    {
        if (value >= 1000000) return $"{value / 1000000:0.##}M";
        if (value >= 1000) return $"{value / 1000:0.##}K";
        return $"{value:0}";
    }
}
