using UnityEngine;

[CreateAssetMenu(fileName = "NewResearchItem", menuName = "Ecogarden/Research Item")]
public class ResearchItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string researchId;
    public string researchName;
    [TextArea] public string description;

    [Header("Cost & Unlocks")]
    public double energyCost = 100;
    public string unlocksTechId;
    public string storeItemUnlockName;
}
