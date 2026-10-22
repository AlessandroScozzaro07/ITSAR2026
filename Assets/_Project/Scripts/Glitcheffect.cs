using UnityEngine;

/// <summary>
/// Effetto glitch: a intervalli casuali l'oggetto va in "burst" per una frazione di secondo,
/// con scatti di posizione e rotazione e sfarfallio di visibilità.
/// Da assegnare all'oggetto con la mesh (es. Mesh, figlio di Core).
/// Non tocca scala ed emission, quindi convive con SquashStretch e PulseLight.
/// </summary>
public class GlitchEffect : MonoBehaviour
{
    [Header("Frequenza")]
    [Tooltip("Attesa minima tra un glitch e il successivo, in secondi.")]
    [SerializeField, Min(0f)] private float minInterval = 2f;

    [Tooltip("Attesa massima tra un glitch e il successivo, in secondi.")]
    [SerializeField, Min(0f)] private float maxInterval = 5f;

    [Tooltip("Durata di ogni glitch, in secondi.")]
    [SerializeField, Min(0.01f)] private float burstDuration = 0.3f;

    [Tooltip("Ogni quanti secondi cambia lo scatto durante il glitch (più basso = più nervoso).")]
    [SerializeField, Min(0.005f)] private float tickInterval = 0.04f;

    [Header("Intensità")]
    [Tooltip("Spostamento massimo degli scatti (soprattutto in orizzontale).")]
    [SerializeField, Min(0f)] private float positionJitter = 0.15f;

    [Tooltip("Rotazione casuale massima per asse, in gradi.")]
    [SerializeField, Min(0f)] private float rotationJitter = 20f;

    [Tooltip("Probabilità (0-1) che a ogni scatto l'oggetto sparisca per un istante.")]
    [SerializeField, Range(0f, 1f)] private float flickerChance = 0.3f;

    [Header("Opzioni")]
    [Tooltip("Se attivo, il primo glitch parte subito all'avvio.")]
    [SerializeField] private bool glitchOnStart = false;

    private Renderer targetRenderer;
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    private bool glitching;
    private float burstEndTime;
    private float nextTickTime;
    private float nextBurstTime;

    private void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
    }

    private void OnEnable()
    {
        ScheduleNextBurst(glitchOnStart);
    }

    private void OnDisable()
    {
        EndGlitch();
    }

    /// <summary>Avvia subito un glitch (richiamabile da altri script, es. quando il cristallo prende danno).</summary>
    public void TriggerGlitch()
    {
        glitching = true;
        burstEndTime = Time.time + burstDuration;
        nextTickTime = 0f;
    }

    private void Update()
    {
        if (!glitching)
        {
            if (Time.time >= nextBurstTime)
            {
                TriggerGlitch();
            }
            return;
        }

        if (Time.time >= burstEndTime)
        {
            EndGlitch();
            ScheduleNextBurst(false);
            return;
        }

        if (Time.time >= nextTickTime)
        {
            ApplyRandomGlitch();
            nextTickTime = Time.time + tickInterval;
        }
    }

    private void ApplyRandomGlitch()
    {
        // Scatti soprattutto orizzontali: è il classico look "glitch".
        Vector3 offset = new Vector3(
            Random.Range(-1f, 1f) * positionJitter,
            Random.Range(-1f, 1f) * positionJitter * 0.3f,
            0f);
        transform.localPosition = startLocalPosition + offset;

        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(-rotationJitter, rotationJitter),
            Random.Range(-rotationJitter, rotationJitter),
            Random.Range(-rotationJitter, rotationJitter));
        transform.localRotation = startLocalRotation * randomRotation;

        if (targetRenderer != null)
        {
            targetRenderer.enabled = Random.value > flickerChance;
        }
    }

    private void EndGlitch()
    {
        glitching = false;
        transform.localPosition = startLocalPosition;
        transform.localRotation = startLocalRotation;
        if (targetRenderer != null)
        {
            targetRenderer.enabled = true;
        }
    }

    private void ScheduleNextBurst(bool immediately)
    {
        float wait = immediately ? 0f : Random.Range(minInterval, Mathf.Max(minInterval, maxInterval));
        nextBurstTime = Time.time + wait;
    }
}