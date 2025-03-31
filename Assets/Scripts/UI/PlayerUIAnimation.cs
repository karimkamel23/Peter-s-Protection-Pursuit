using UnityEngine;
using System.Collections;

public class PlayerUIAnimation : MonoBehaviour
{

    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float moveDistanceX = 13.3f; 
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void PlayPlayerAnimation()
    {
        anim.SetTrigger("Run");
        StartCoroutine(MovePlayer());

    }

    private IEnumerator MovePlayer()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x + moveDistanceX, startPosition.y, startPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}
