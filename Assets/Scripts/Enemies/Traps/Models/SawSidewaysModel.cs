using UnityEngine;

[System.Serializable]
public class SawSidewaysModel
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    
    // State
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    
    public float MovementDistance => movementDistance;
    public float Speed => speed;
    
    public bool MovingLeft 
    { 
        get => movingLeft; 
        set => movingLeft = value; 
    }
    
    public float LeftEdge 
    { 
        get => leftEdge; 
        set => leftEdge = value; 
    }
    
    public float RightEdge 
    { 
        get => rightEdge; 
        set => rightEdge = value; 
    }
} 