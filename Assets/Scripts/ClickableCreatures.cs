using UnityEngine;

public class ClickableCreature : MonoBehaviour
{
    [SerializeField] private double baseEnergyPerClick = 1;
    [SerializeField] private ParticleSystem clickBurst; 
    [SerializeField] private Animator animator;        

    void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        float mult = 1.0f;
        if (VillagerSatisfactionSystem.Instance != null)
        {
            mult = VillagerSatisfactionSystem.Instance.CurrentMultiplier;
            VillagerSatisfactionSystem.Instance.RegisterVillagerClick();
        }

        double earned = baseEnergyPerClick * mult;
        if (EnergyManager.Instance != null)
        {
            EnergyManager.Instance.AddEnergy(earned);
        }

        if (clickBurst) clickBurst.Play();
        if (animator) animator.SetTrigger("Click");
        Debug.Log($"[ClickableCreature] Clicked! Earned {earned:0.#} Energy (Multiplier: {mult:0.##}x)");
    }
}