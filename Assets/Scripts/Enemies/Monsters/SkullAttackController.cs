using System.Collections;
using UnityEngine;

public class SkullAttack : MeleeEnemy
{
    public float attackSpeed = 0.2f;  // Speed for movement transitions

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private int facingDirection = 1; // 1 = Right, -1 = Left

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("attack");
            TriggerAttack();
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    public void TriggerAttack()
    {
        // Determine facing direction (-1 if flipped, 1 if normal)
        facingDirection = (transform.localScale.x < 0) ? -1 : 1;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // PHASE 1: Move Up & Rotate (-1.68 -> -1.34) (0° -> -17.35°)
        Vector3 phase1Pos = new Vector3(startPos.x, startPos.y + (1.68f - 1.34f), startPos.z);
        Quaternion phase1Rot = Quaternion.Euler(0, 0, facingDirection * -17.35f);
        yield return MoveAndRotate(phase1Pos, phase1Rot, attackSpeed);

        // PHASE 2: Move Forward & Slightly Down (19.24 -> 19.86) (-1.34 -> -1.67)
        Vector3 phase2Pos = new Vector3(startPos.x + facingDirection * (19.86f - 19.24f), startPos.y - (1.67f - 1.34f), startPos.z);
        Quaternion phase2Rot = phase1Rot; // Keep rotation the same
        yield return MoveAndRotate(phase2Pos, phase2Rot, attackSpeed);

        // PHASE 3: Hold Position (Frame 40-100)
        yield return new WaitForSeconds(0.4f);

        // PHASE 4: Return to Initial Position & Reset Rotation
        yield return MoveAndRotate(initialPosition, initialRotation, attackSpeed);
    }

    IEnumerator MoveAndRotate(Vector3 targetPos, Quaternion targetRot, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsedTime); // Ensure rotation is being applied
            elapsedTime += Time.deltaTime / duration;
            yield return null;
        }

        // Ensure final values are set
        transform.position = targetPos;
        transform.rotation = targetRot;
    }
}
