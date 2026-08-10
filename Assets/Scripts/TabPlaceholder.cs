using UnityEngine;

public class TabPlaceholder : MonoBehaviour
{
    [SerializeField] private string tabName;

    public void OnTabClicked()
    {
        Debug.Log($"{tabName} — coming soon");
    }
}