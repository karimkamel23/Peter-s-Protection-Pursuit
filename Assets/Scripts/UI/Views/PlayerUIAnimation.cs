using UnityEngine;
using System.Collections;

public class PlayerUIAnimation : MonoBehaviour
{
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float moveDistanceX = 13.3f;
    private Animator anim;
    private Vector3 originalScale; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    public void PlayPlayerAnimation()
    {
        StartCoroutine(MovePlayer(direction: 1));
    }

    public void PlayReverseAnimation()
    {
        StartCoroutine(MovePlayer(direction: -1));
    }

    private IEnumerator MovePlayer(int direction)
    {
        anim.SetTrigger("Run");

        // Flip player based on direction
        Vector3 flippedScale = originalScale;
        flippedScale.x *= direction;
        transform.localScale = flippedScale;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x + moveDistanceX * direction, startPosition.y, startPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;

        // Restore facing direction to original (+x)
        transform.localScale = originalScale;
    }
}
