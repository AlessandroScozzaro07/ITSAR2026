using UnityEngine;

/// <summary>
/// Squash and stretch: l'oggetto si allunga in verticale mentre sale o scende velocemente
/// e si schiaccia quando atterra. Legge la velocità verticale dalla posizione, quindi
/// funziona con qualsiasi movimento (es. Jumper sul genitore).
/// Da assegnare all'oggetto con la mesh (es. Mesh, figlio di Core).
/// </summary>
public class SquashStretch : MonoBehaviour
{
    [Header("Intensità")]
    [Tooltip("Quanto si allunga in verticale alla massima velocità (0.3 = +30%).")]
    [SerializeField, Min(0f)] private float stretchAmount = 0.3f;

    [Tooltip("Quanto si schiaccia all'atterraggio più forte (0.4 = -40%).")]
    [SerializeField, Range(0f, 0.9f)] private float squashAmount = 0.4f;

    [Header("Sensibilità")]
    [Tooltip("Velocità verticale (unità/secondo) a cui l'effetto è al massimo. Calcolala come 4 x altezza / durata del salto.")]
    [SerializeField, Min(0.1f)] private float referenceSpeed = 3f;

    [Tooltip("Velocità minima di discesa per considerare un atterraggio.")]
    [SerializeField, Min(0f)] private float landingSpeedThreshold = 0.5f;

    [Header("Animazione")]
    [Tooltip("Velocità con cui la forma torna normale. Più alto = più scattante.")]
    [SerializeField, Min(0.1f)] private float recoverySpeed = 14f;

    [Tooltip("Se attivo, compensa X e Z per mantenere il volume costante.")]
    [SerializeField] private bool preserveVolume = true;

    private Vector3 baseScale;
    private float lastY;
    private float lastVelocityY;
    private float shape; // > 0 allungato, < 0 schiacciato

    private void Awake()
    {
        baseScale = transform.localScale;
        lastY = transform.position.y;
    }

    private void OnDisable()
    {
        transform.localScale = baseScale;
        shape = 0f;
        lastVelocityY = 0f;
    }

    // LateUpdate: il movimento (es. Jumper) è già stato applicato in Update.
    private void LateUpdate()
    {
        float dt = Time.deltaTime;
        if (dt <= 0f) return;

        float y = transform.position.y;
        float velocityY = (y - lastY) / dt;

        // Atterraggio: stava scendendo veloce e ora si è fermato o risale.
        bool wasFalling = lastVelocityY < -landingSpeedThreshold;
        bool stoppedFalling = velocityY > -landingSpeedThreshold * 0.5f;
        if (wasFalling && stoppedFalling)
        {
            float impact = Mathf.Clamp01(-lastVelocityY / referenceSpeed);
            shape = -squashAmount * impact;
        }

        // In aria: più veloce si muove, più si allunga.
        float speed01 = Mathf.Clamp01(Mathf.Abs(velocityY) / referenceSpeed);
        float target = stretchAmount * speed01;

        // Avvicinamento fluido al valore target (indipendente dal framerate).
        float t = 1f - Mathf.Exp(-recoverySpeed * dt);
        shape = Mathf.Lerp(shape, target, t);

        float scaleY = 1f + shape;
        float scaleXZ = preserveVolume ? 1f / Mathf.Sqrt(scaleY) : 1f;

        transform.localScale = new Vector3(
            baseScale.x * scaleXZ,
            baseScale.y * scaleY,
            baseScale.z * scaleXZ);

        lastY = y;
        lastVelocityY = velocityY;
    }
}