using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClosePanelButton : MonoBehaviour
{
    void Awake()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnCloseClicked);
        }
    }

    public void OnCloseClicked()
    {
        if (TabManager.Instance != null)
        {
            TabManager.Instance.CloseAllPanels();
        }
        else
        {
            // Fallback if TabManager not initialized: hide parent panel directly
            Transform parentPanel = transform.parent;
            if (parentPanel != null)
            {
                parentPanel.gameObject.SetActive(false);
            }
        }
    }
}
