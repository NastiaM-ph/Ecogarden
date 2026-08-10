using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text perSecondText;

    void Start()
    {
        EnergyManager.Instance.OnEnergyChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        if (EnergyManager.Instance != null)
            EnergyManager.Instance.OnEnergyChanged -= Refresh;
    }

    void Refresh()
    {
        energyText.text = FormatNumber(EnergyManager.Instance.CurrentEnergy);
        perSecondText.text = $"{FormatNumber(EnergyManager.Instance.EnergyPerSecond)} / sec";
    }

    string FormatNumber(double value)
    {
        if (value >= 1000) return $"{value / 1000:0.##}K";
        return $"{value:0}";
    }
}