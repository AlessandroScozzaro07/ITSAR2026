using UnityEngine;

public enum AreaType
{
    Damage, // danno nel tempo mentre il player è sopra
    Heal    // cura periodica mentre il player è sopra
}

/// <summary>Chi può subire gli effetti di movimento delle aree (implementato da PlayerController).</summary>
public interface IMovementEffectReceiver
{
    void ApplySlide(float acceleration, float duration);
    void ApplySlow(float multiplier, float duration);
    void ApplySpeedBoost(float multiplier, float duration);
}

/// <summary>
/// Zona con collider trigger: infligge danno o cura a intervalli regolari al Player.
/// Può applicare effetti di movimento opzionali (scivolamento, rallentamento, accelerazione)
/// che restano attivi per effectDuration secondi. Finché il player è nella zona la durata
/// viene rinnovata, quindi i secondi iniziano a scorrere quando esce.
/// Collider (Is Trigger) e AreaEffect devono stare sullo stesso oggetto.
/// </summary>
[RequireComponent(typeof(Collider))]
public class AreaEffect : MonoBehaviour
{
    [Header("Tipo di zona")]
    [SerializeField] private AreaType areaType = AreaType.Damage;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField, Min(0f)] private float amountPerTick = 5f;
    [SerializeField, Min(0.05f)] private float tickInterval = 1f;

    [Header("Effetti opzionali di movimento")]
    [SerializeField, Min(0f)] private float effectDuration = 5f;

    [SerializeField] private bool applySlide;
    [SerializeField, Tooltip("Accelerazione orizzontale mentre scivola: più è bassa, più scivola")]
    private float slideAcceleration = 3f;

    [SerializeField] private bool applySlow;
    [SerializeField, Range(0.05f, 1f)] private float slowMultiplier = 0.5f;

    [SerializeField] private bool applySpeedBoost;
    [SerializeField, Min(1f)] private float speedBoostMultiplier = 1.5f;

    private float nextTickTime;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
        targetLayer = LayerMask.GetMask("Player");
    }

    private void OnValidate()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"{name}: il collider di AreaEffect deve avere Is Trigger attivo.", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTarget(other)) return;
        nextTickTime = Time.time; // primo tick immediato
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsTarget(other)) return;

        IMovementEffectReceiver receiver = other.GetComponentInParent<IMovementEffectReceiver>();
        if (receiver != null)
        {
            ApplyMovementEffects(receiver);
        }

        if (Time.time >= nextTickTime)
        {
            nextTickTime = Time.time + tickInterval;
            ApplyTick(other);
        }
    }

    private bool IsTarget(Collider other)
    {
        return (targetLayer.value & (1 << other.gameObject.layer)) != 0;
    }

    private void ApplyTick(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health == null || health.isDead) return;

        if (areaType == AreaType.Damage)
        {
            health.TakeDamage(amountPerTick);
        }
        else
        {
            health.Heal(amountPerTick);
        }
    }

    private void ApplyMovementEffects(IMovementEffectReceiver receiver)
    {
        if (applySlide) receiver.ApplySlide(slideAcceleration, effectDuration);
        if (applySlow) receiver.ApplySlow(slowMultiplier, effectDuration);
        if (applySpeedBoost) receiver.ApplySpeedBoost(speedBoostMultiplier, effectDuration);
    }
}