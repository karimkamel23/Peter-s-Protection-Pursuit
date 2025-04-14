using System.Collections;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    [Header("Death Animation Settings")]
    public float deathDuration = 0.4f; // Total animation time

    private Vector3 initialScale;
    private Quaternion initialRotation;
    private int facingDirection; // 1 = right, -1 = left

    public void PlayDeathAnimation()
    {
        // Determine facing direction dynamically at death time
        facingDirection = (transform.localScale.x < 0) ? -1 : 1;

        // Store initial state at the moment of death
        initialScale = transform.localScale;
        initialRotation = transform.rotation;

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // Target values relative to current state
        Vector3[] targetScales = new Vector3[]
        {
            initialScale, // 0:00
            initialScale, // 0:15
            initialScale * (0.1f / 0.15f), // 0:30
            initialScale * (0.05f / 0.15f), // 0:35
            initialScale * (0.01f / 0.15f) // 0:40
        };

        Quaternion[] targetRotations = new Quaternion[]
        {
            initialRotation, // 0:00 (No Rotation)
            Quaternion.Euler(0, 0, 27 * facingDirection) * initialRotation, // 0:15
            Quaternion.Euler(0, 0, 45 * facingDirection) * initialRotation, // 0:30
            Quaternion.Euler(0, 0, 45 * facingDirection) * initialRotation, // 0:35
            Quaternion.Euler(0, 0, 45 * facingDirection) * initialRotation  // 0:40
        };

        float[] keyframeDurations = { 0.15f, 0.15f, 0.05f, 0.05f };

        for (int i = 0; i < keyframeDurations.Length; i++)
        {
            yield return AnimateStep(targetScales[i + 1], targetRotations[i + 1], keyframeDurations[i]);
        }

    }

    private IEnumerator AnimateStep(Vector3 targetScale, Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        transform.localScale = targetScale;
        transform.rotation = targetRotation;
    }
}
