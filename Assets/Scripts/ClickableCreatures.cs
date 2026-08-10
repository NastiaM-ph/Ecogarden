using UnityEngine;

public class ClickableCreature : MonoBehaviour
{
    [SerializeField] private double energyPerClick = 1;
    [SerializeField] private ParticleSystem clickBurst; 
    [SerializeField] private Animator animator;        

    void OnMouseDown()
    {
        Debug.Log("Creature clicked!");
        EnergyManager.Instance.AddEnergy(energyPerClick);

        if (clickBurst) clickBurst.Play();
        if (animator) animator.SetTrigger("Click");
    }
}