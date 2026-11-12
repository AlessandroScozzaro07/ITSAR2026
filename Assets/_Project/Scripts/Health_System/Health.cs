using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;

    float currenthealth;

    public float CurrentHealth => currenthealth;
    public float MaxHealth => maxHealth;

    public bool isDead { get; private set; }
    public Action<float, float> OnHealChanged;  // evento in cui si può sottoscrivere (vita, vita massima)
    public Action Died;

    private void Awake()
    {
        currenthealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }
        currenthealth = Mathf.Clamp(currenthealth - damage, 0f, maxHealth);
        Debug.Log($" HP: {currenthealth}/{maxHealth}");
        OnHealChanged?.Invoke(currenthealth, maxHealth);
        if (currenthealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float heal)
    {
        if (isDead)
        {
            return;
        }
        // Clamp restituisce il valore confinato: va assegnato, altrimenti non ha effetto.
        currenthealth = Mathf.Clamp(currenthealth + heal, 0f, maxHealth);
        Debug.Log($" HEAL HP: {currenthealth}/{maxHealth}");
        OnHealChanged?.Invoke(currenthealth, maxHealth);
    }

    public void SetHealth(float _health, float _maxHealth)
    {
        maxHealth = _maxHealth;
        currenthealth = Mathf.Clamp(_health, 0f, maxHealth);
        OnHealChanged?.Invoke(currenthealth, maxHealth);
    }

    void Die()
    {
        isDead = true;
        Died?.Invoke();
        gameObject.SetActive(false);
    }
}