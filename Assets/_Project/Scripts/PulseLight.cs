using UnityEngine;

/// <summary>
/// Fa pulsare l'intensità dell'emission del materiale (URP Lit) con un'onda sinusoidale.
/// Da assegnare all'oggetto che contiene il Renderer del cristallo (es. Core).
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PulseLight : MonoBehaviour
{
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    [Header("Emission")]
    [Tooltip("Colore base dell'emission (l'intensità viene applicata da min/max).")]
    [SerializeField, ColorUsage(false, false)] private Color emissionColor = Color.cyan;

    [Tooltip("Intensità quando il cristallo è 'spento' (minimo dell'onda).")]
    [SerializeField, Min(0f)] private float minIntensity = 0.5f;

    [Tooltip("Intensità quando il cristallo è 'acceso' (massimo dell'onda).")]
    [SerializeField, Min(0f)] private float maxIntensity = 3f;

    [Header("Pulsazione")]
    [Tooltip("Numero di pulsazioni complete al secondo (Hz).")]
    [SerializeField, Min(0f)] private float pulsesPerSecond = 0.5f;

    [Tooltip("Indice del materiale da animare, se il Renderer ne ha più di uno.")]
    [SerializeField, Min(0)] private int materialIndex = 0;

    private Material materialInstance;

    private void Awake()
    {
        // .materials restituisce istanze: il materiale condiviso nel progetto non viene modificato.
        Material[] materials = GetComponent<Renderer>().materials;
        materialIndex = Mathf.Clamp(materialIndex, 0, materials.Length - 1);
        materialInstance = materials[materialIndex];

        // Necessario perché l'emission venga renderizzata anche se la casella sul materiale è spenta.
        materialInstance.EnableKeyword("_EMISSION");
        materialInstance.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
    }

    private void Update()
    {
        // Sinusoide normalizzata 0..1, parte dal minimo.
        float wave = 0.5f - 0.5f * Mathf.Cos(2f * Mathf.PI * pulsesPerSecond * Time.time);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, wave);

        // _EmissionColor lavora in spazio lineare.
        materialInstance.SetColor(EmissionColorId, emissionColor.linear * intensity);
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}
