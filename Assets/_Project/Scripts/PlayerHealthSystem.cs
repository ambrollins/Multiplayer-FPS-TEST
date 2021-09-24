using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private const float MaxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private HealthView healthView;

    private void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(float damageAmount)
    {
        ReduceHealth(damageAmount);
    }

    public void ReduceHealth(float amount)
    {
        float _currentHealth = GetHealth();
        _currentHealth -= amount;
        SetHealth(_currentHealth);
    }

    public void IncreaseHealth(float amount)
    {
        float _currentHealth = GetHealth();
        _currentHealth += amount;
        SetHealth(_currentHealth);
    }

    private void ResetHealth()
    {
        SetHealth(MaxHealth);
    }
    
    private void SetHealth(float newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, MaxHealth);
        currentHealth = newHealth;
        healthView.UpdateView(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
