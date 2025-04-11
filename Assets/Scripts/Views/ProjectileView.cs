using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    private Animator animator;
    private ProjectileModel model;
    private Transform projectileTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        model = GetComponent<ProjectileModel>();
        projectileTransform = transform;
    }

    private void Start()
    {
        if (model != null)
        {
            model.OnProjectileHit += PlayExplosionAnimation;
        }
    }

    public void UpdatePosition(float movementAmount)
    {
        projectileTransform.Translate(movementAmount, 0, 0);
    }

    public void UpdateVisualDirection(float direction)
    {
        Vector3 currentScale = projectileTransform.localScale;
        float localScaleX = Mathf.Abs(currentScale.x);
        
        if (direction < 0)
            localScaleX = -localScaleX;
        else
            localScaleX = Mathf.Abs(localScaleX);

        projectileTransform.localScale = new Vector3(localScaleX, currentScale.y, currentScale.z);
    }

    private void PlayExplosionAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("explode");
        }
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnProjectileHit -= PlayExplosionAnimation;
        }
    }
} 