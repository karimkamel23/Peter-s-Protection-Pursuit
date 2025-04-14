using UnityEngine;

public class SawSidewaysController : EnemyDamage
{
    [SerializeField] private SawSidewaysModel model;

    private void Awake()
    {
        // Calculate movement boundaries
        model.LeftEdge = transform.position.x - model.MovementDistance;
        model.RightEdge = transform.position.x + model.MovementDistance;
    }

    private void Update()
    {
        if (model.MovingLeft) 
        {
            if (transform.position.x > model.LeftEdge)
            {
                transform.position = new Vector3(
                    transform.position.x - (model.Speed * Time.deltaTime), 
                    transform.position.y, 
                    transform.position.z);
            }
            else model.MovingLeft = false;
        }
        else
        {
            if (transform.position.x < model.RightEdge)
            {
                transform.position = new Vector3(
                    transform.position.x + (model.Speed * Time.deltaTime), 
                    transform.position.y, 
                    transform.position.z);
            }
            else model.MovingLeft = true;
        }
    }
} 