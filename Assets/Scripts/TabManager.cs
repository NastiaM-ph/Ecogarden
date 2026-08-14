using UnityEngine;

public enum UIPanelType
{
    None,
    Store,
    Research,
    Stats,
    Villagers
}

public class TabManager : MonoBehaviour
{
    public static TabManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject panelStore;
    [SerializeField] private GameObject panelResearch;
    [SerializeField] private GameObject panelStats;
    [SerializeField] private GameObject panelVillagers;

    [Header("Current State")]
    [SerializeField] private UIPanelType activePanel = UIPanelType.None;

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
        CloseAllPanels();
    }

    public void OpenStore() => ShowPanel(UIPanelType.Store);
    public void OpenResearch() => ShowPanel(UIPanelType.Research);
    public void OpenStats() => ShowPanel(UIPanelType.Stats);
    public void OpenVillagers() => ShowPanel(UIPanelType.Villagers);
    public void CloseAllPanels() => ShowPanel(UIPanelType.None);

    public void ShowPanel(UIPanelType panelType)
    {
        activePanel = panelType;

        if (panelStore != null) panelStore.SetActive(panelType == UIPanelType.Store);
        if (panelResearch != null) panelResearch.SetActive(panelType == UIPanelType.Research);
        if (panelStats != null) panelStats.SetActive(panelType == UIPanelType.Stats);
        if (panelVillagers != null) panelVillagers.SetActive(panelType == UIPanelType.Villagers);

        Debug.Log($"[TabManager] Active Panel set to: {panelType}");
    }

    public void SelectTabByName(string tabName)
    {
        switch (tabName.ToLower())
        {
            case "generators":
            case "store":
            case "shop":
                OpenStore();
                break;
            case "research":
                OpenResearch();
                break;
            case "stats":
                OpenStats();
                break;
            case "villagers":
            case "creatures":
                OpenVillagers();
                break;
            case "world":
            case "close":
                CloseAllPanels();
                break;
            default:
                Debug.LogWarning($"[TabManager] Unknown tab name: {tabName}");
                break;
        }
    }
}
