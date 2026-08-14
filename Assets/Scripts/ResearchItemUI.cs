using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button researchButton;

    private ResearchItemData itemData;
    private System.Action<ResearchItemData> onResearchClickedCallback;

    public void Setup(ResearchItemData data, bool isResearched, System.Action<ResearchItemData> onResearchClicked)
    {
        this.itemData = data;
        this.onResearchClickedCallback = onResearchClicked;

        if (researchButton != null)
        {
            researchButton.onClick.RemoveAllListeners();
            researchButton.onClick.AddListener(() => onResearchClickedCallback?.Invoke(itemData));
        }

        RefreshUI(isResearched);
    }

    public void RefreshUI(bool isResearched)
    {
        if (itemData == null) return;

        if (titleText != null) titleText.text = itemData.researchName;
        if (descriptionText != null) descriptionText.text = itemData.description;
        if (costText != null) costText.text = FormatNumber(itemData.energyCost);

        if (isResearched)
        {
            if (statusText != null) statusText.text = "RESEARCHED";
            if (researchButton != null) researchButton.interactable = false;
        }
        else
        {
            if (statusText != null) statusText.text = "RESEARCH";
            if (EnergyManager.Instance != null && researchButton != null)
            {
                researchButton.interactable = EnergyManager.Instance.CurrentEnergy >= itemData.energyCost;
            }
        }
    }

    private string FormatNumber(double value)
    {
        if (value >= 1000000) return $"{value / 1000000:0.##}M";
        if (value >= 1000) return $"{value / 1000:0.##}K";
        return $"{value:0}";
    }
}
