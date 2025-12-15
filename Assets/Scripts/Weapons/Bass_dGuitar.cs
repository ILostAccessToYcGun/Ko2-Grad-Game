using System.Collections;
using Unity.Jobs;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Bass_dGuitar : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float speed;
    [SerializeField] float duration;
    [SerializeField] float m1Knockback;
    [SerializeField] float angleSpread = 10.0f;
    [SerializeField] float dot = 0.5f;
    [SerializeField] float sizeGain = 0.1f;

    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.5f;
    bool isSwingRight;
    [SerializeField] float m2Knockback;
    [SerializeField] float stunTime;
    [SerializeField] float offset;
    [SerializeField] float startDistance;
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
                        BassProjectile bass =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity)
                            .GetComponent<BassProjectile>();

                        if (isFlippedSide)
                            bass.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                        else
                            bass.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);

                        bass.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        bass.speed = speed;
                        bass.knockback = m1Knockback;
                        bass.DOTInterval = dot;
                        bass.sizeGain = sizeGain;
                        bass.duration = duration;
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
                        BassProjectile bass =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity)
                            .GetComponent<BassProjectile>();

                        if (i == 0)
                        {
                            bass.moveDir = forwardVector;
                        }
                        else
                        {
                            if (isFlippedSide)
                                bass.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                            else
                                bass.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);
                        }

                        bass.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        bass.speed = speed;
                        bass.knockback = m1Knockback;
                        bass.DOTInterval = dot;
                        bass.sizeGain = sizeGain;
                        bass.duration = duration;
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
                BassSmash smash =
                            Instantiate(m2Projectile, transform.position, Quaternion.identity, transform)
                            .GetComponentInChildren<BassSmash>();
                smash.moveDir = forwardVector;
                HelperManager.instance.RotateTowardsDirection((GameManager.instance.cam.screenToWorld - (Vector2)GameManager.instance.playerMovement.transform.position).normalized, smash.transform.parent);
                smash.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                smash.knockback = m2Knockback;
                smash.stunTime = stunTime;
                smash.offset = offset;
                smash.startDistance = startDistance;


                if (isSwingRight) smash.swingAnimation.clip = smash.right;
                else smash.swingAnimation.clip = smash.left;
                smash.swingAnimation.Play();

                isSwingRight = !isSwingRight;

                attackTimer = m2CD;
            }
        }
    }
}
