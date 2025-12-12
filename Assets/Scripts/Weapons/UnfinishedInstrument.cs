using System.Collections;
using Unity.Jobs;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class UnfinishedInstrument : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float bulletSpeed;
    [SerializeField] float oscillationSpeed;
    [SerializeField] float oscillationRange;
    [SerializeField] int pierce;
    [SerializeField] float duration;
    [SerializeField] float angleSpread = 10.0f;

    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.5f;
    protected override void OnUpdate()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime * GameManager.instance.playerStats.ATKSPD;
        }
        else
        {
            if (GameManager.instance.gameState != GameManager.States.Playing) return;
            Vector2 forwardVector = (GameManager.instance.cam.screenToWorld - (Vector2)playerMovement.transform.position).normalized;
            if (m1.ReadValue<float>() > 0.0f)
            {
                //M1 attack
                bool isFlippedSide = false;
                bool isEven = GameManager.instance.playerStats.PRJ % 2 == 0 ? true : false;
                int flipCounter = 0;
                float currentSpread = 0.0f;
                float angle = angleSpread;

                angle = angle / (GameManager.instance.playerStats.PRJ * 0.25f);

                if (isEven)
                {
                    currentSpread += angle * 0.5f;
                    for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
                    {
                        MusicNoteProjectile note =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity)
                            .GetComponent<MusicNoteProjectile>();

                        if (isFlippedSide)
                            note.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                        else
                            note.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);

                        note.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        note.speed = bulletSpeed;
                        note.oscillationSpeed = oscillationSpeed;
                        note.oscillationRange = oscillationRange;
                        note.pierce = pierce;
                        note.duration = duration;
                        attackTimer = m1CD;

                        isFlippedSide = !isFlippedSide;
                        flipCounter++;
                        if (flipCounter >= 2) currentSpread += angle;
                    }
                }
                else
                {
                    flipCounter = 1;
                    for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
                    {
                        MusicNoteProjectile note =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity)
                            .GetComponent<MusicNoteProjectile>();

                        if (i == 0)
                        {
                            note.moveDir = forwardVector;
                        }
                        else
                        {
                            if (isFlippedSide)
                                note.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                            else
                                note.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);
                        }

                        note.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        note.speed = bulletSpeed;
                        note.oscillationSpeed = oscillationSpeed;
                        note.oscillationRange = oscillationRange;
                        note.pierce = pierce;
                        note.duration = duration;
                        attackTimer = m1CD;

                        isFlippedSide = !isFlippedSide;
                        flipCounter++;
                        if (flipCounter >= 2) currentSpread += angle;
                    }
                }
            }
            
            else if (m2.ReadValue<float>() > 0.0f)
            {
                //M2 attack
                StartCoroutine(SlamAttack(GameManager.instance.cam.screenToWorld, forwardVector));
                attackTimer = m2CD;
            }
        }
    }


    IEnumerator SlamAttack(Vector2 pos, Vector2 forward)
    {
        float timer = 0.2f;
        
        for ( int i = 0; i < GameManager.instance.playerStats.PRJ; i++ )
        {
            MusicNoteSlam note =
                            Instantiate(m2Projectile, pos, Quaternion.identity)
                            .GetComponentInChildren<MusicNoteSlam>();

            note.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
            note.moveDir = forward;

            timer = 0.1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        yield return null;
    }
}
