using UnityEngine;
using TMPro;

public class StoreCategoryHeaderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    public void Setup(string title)
    {
        if (titleText != null)
        {
            titleText.text = title;
        }
    }
}
