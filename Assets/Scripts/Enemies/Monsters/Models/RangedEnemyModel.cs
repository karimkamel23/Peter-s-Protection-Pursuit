using UnityEngine;

[System.Serializable]
public class RangedEnemyModel
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Projectile Sound")]
    [SerializeField] private AudioClip projectileSound;

    // State
    private float cooldownTimer = Mathf.Infinity;

    // Properties
    public float AttackCooldown => attackCooldown;
    public float Range => range;
    public int Damage => damage;
    public Transform FirePoint => firePoint;
    public GameObject[] Projectiles => projectiles;
    public float ColliderDistance => colliderDistance;
    public LayerMask PlayerLayer => playerLayer;
    public AudioClip ProjectileSound => projectileSound;

    public float CooldownTimer
    {
        get => cooldownTimer;
        set => cooldownTimer = value;
    }
} 