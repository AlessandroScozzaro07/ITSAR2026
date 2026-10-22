using UnityEngine;

/// <summary>
/// Fa saltare l'oggetto in verticale con una traiettoria parabolica, in loop.
/// L'altezza, la durata del salto e la pausa tra un salto e l'altro sono configurabili.
/// </summary>
public class Jumper : MonoBehaviour
{
    [Tooltip("Altezza massima del salto, in unità Unity.")]
    [SerializeField, Min(0f)] private float jumpHeight = 0.5f;

    [Tooltip("Durata di un salto (da terra a terra), in secondi.")]
    [SerializeField, Min(0.05f)] private float jumpDuration = 0.8f;

    [Tooltip("Pausa a terra tra un salto e il successivo, in secondi.")]
    [SerializeField, Min(0f)] private float pauseBetweenJumps = 0.2f;

    private Vector3 startLocalPosition;
    private float timer;

    private void Awake()
    {
        // Posizione di partenza: il salto parte e torna sempre da qui.
        startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float cycleDuration = jumpDuration + pauseBetweenJumps;
        float timeInCycle = timer % cycleDuration;

        float offsetY = 0f;
        if (timeInCycle < jumpDuration)
        {
            // n va da 0 a 1 durante il salto.
            float n = timeInCycle / jumpDuration;

            // Parabola: 0 a terra, 1 al punto più alto (n = 0.5).
            offsetY = 4f * n * (1f - n) * jumpHeight;
        }

        transform.localPosition = startLocalPosition + Vector3.up * offsetY;
    }
}
