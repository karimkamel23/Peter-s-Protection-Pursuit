using UnityEngine;

[System.Serializable]
public class MeleeEnemyModel
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private bool top;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;

    // State
    private float cooldownTimer = Mathf.Infinity;

    // Properties
    public float AttackCooldown => attackCooldown;
    public float Range => range;
    public int Damage => damage;
    public float ColliderDistance => colliderDistance;
    public bool Top => top;
    public LayerMask PlayerLayer => playerLayer;
    public AudioClip AttackSound => attackSound;

    public float CooldownTimer
    {
        get => cooldownTimer;
        set => cooldownTimer = value;
    }
} 