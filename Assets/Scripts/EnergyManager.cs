using UnityEngine;
using System;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }

    public double CurrentEnergy { get; private set; }
    public double EnergyPerSecond { get; private set; } = 0;

    public event Action OnEnergyChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (EnergyPerSecond > 0)
        {
            CurrentEnergy += EnergyPerSecond * Time.deltaTime;
            OnEnergyChanged?.Invoke();
        }
    }

    public void AddEnergy(double amount)
    {
        CurrentEnergy += amount;
        OnEnergyChanged?.Invoke();
    }
}