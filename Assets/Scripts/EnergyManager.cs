using UnityEngine;
using System;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }

    public double CurrentEnergy { get; private set; }
    public double EnergyPerSecond { get; private set; } = 0;
    public double EnergyPerClick { get; private set; } = 1;
    public double TotalLifetimeEnergy { get; private set; } = 0;

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
            double added = EnergyPerSecond * Time.deltaTime;
            CurrentEnergy += added;
            TotalLifetimeEnergy += added;
            OnEnergyChanged?.Invoke();
        }
    }

    public void AddEnergy(double amount)
    {
        CurrentEnergy += amount;
        if (amount > 0) TotalLifetimeEnergy += amount;
        if (CurrentEnergy < 0) CurrentEnergy = 0;
        OnEnergyChanged?.Invoke();
    }

    public void AddEnergyPerSecond(double amount)
    {
        EnergyPerSecond += amount;
        if (EnergyPerSecond < 0) EnergyPerSecond = 0;
        OnEnergyChanged?.Invoke();
    }

    public bool TrySpendEnergy(double amount)
    {
        if (CurrentEnergy >= amount)
        {
            AddEnergy(-amount);
            return true;
        }
        return false;
    }
}