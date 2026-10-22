using UnityEngine;

/// <summary>
/// Fa ruotare l'oggetto sull'asse Y a velocità regolabile.
/// Da assegnare all'oggetto Core (quello che contiene la mesh del cristallo).
/// </summary>
public class Rotator : MonoBehaviour
{
    [Tooltip("Velocità di rotazione in gradi al secondo.")]
    [SerializeField] private float rotationSpeed = 45f;

    /// <summary>Velocità attuale in gradi al secondo (modificabile da altri script).</summary>

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}

