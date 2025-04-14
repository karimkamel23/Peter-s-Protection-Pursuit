using UnityEngine;

[System.Serializable]
public class ArrowTrapModel
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private AudioClip arrowSound;

    private float cooldownTimer;

    public float AttackCooldown => attackCooldown;
    public AudioClip ArrowSound => arrowSound;

    public float CooldownTimer
    {
        get => cooldownTimer;
        set => cooldownTimer = value;
    }

    public bool IsReadyToAttack => cooldownTimer >= attackCooldown;

    public void ResetCooldown()
    {
        cooldownTimer = 0;
    }
} 