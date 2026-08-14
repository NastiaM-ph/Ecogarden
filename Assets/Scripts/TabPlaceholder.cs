using UnityEngine;

public class TabPlaceholder : MonoBehaviour
{
    [SerializeField] private string tabName;

    public void OnTabClicked()
    {
        if (TabManager.Instance != null)
        {
            TabManager.Instance.SelectTabByName(tabName);
        }
        else
        {
            Debug.Log($"[TabPlaceholder] {tabName} clicked, but TabManager instance was not found.");
        }
    }
}