using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    public delegate void HealthChangedDelegate(Health healthComponent, int damageAmountDealt);
    public delegate void HealthBelowZeroDelegate(GameObject thisDied);
    public event HealthChangedDelegate HealthChangedEvent;
    public event HealthBelowZeroDelegate HealthZeroEvent;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public bool IsDead { get { return CheckIfDead(); } }
    public int CurrentHealth { get { return currentHealth; } }
    public int MaxHealth { get { return maxHealth; } }

    public void DealDamage(int health)
    {
        int damageDealtAmount = health;
        if (currentHealth - health <= 0)
        {
            damageDealtAmount = currentHealth;
        }
        currentHealth -= health;
        OnHealthChanged(damageDealtAmount);
        if (currentHealth <= 0)
        {
            HealthZeroEvent?.Invoke(gameObject);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private bool CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    private void OnHealthChanged(int damageAmountDealt)
    {
        if (HealthChangedEvent != null)
        {
            HealthChangedEvent(this, damageAmountDealt);
        }

    }
}
