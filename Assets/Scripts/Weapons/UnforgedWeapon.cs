using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class UnforgedWeapon : WeaponBase
{[SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float m1Knockback;
    bool isSwingRight;



    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.0f;
    [SerializeField] float m2Knockback;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float damageInterval;
    [SerializeField] float duration;
    [SerializeField] float angleSpread = 45.0f;
    [SerializeField] float rotationSpeed = 300f;

    [Header("Particles")]
    [SerializeField] GameObject spark; // ?
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
                StartCoroutine(SwingAttack(forwardVector));
                attackTimer = m1CD;

            }

            else if (m2.ReadValue<float>() > 0.0f)
            {
                //M2 attack
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
                        SpinSwordProjectile boom =
                            Instantiate(m2Projectile, transform.position, Quaternion.identity)
                            .GetComponent<SpinSwordProjectile>();

                        if (isFlippedSide)
                            boom.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                        else
                            boom.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);

                        boom.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                        boom.speed = speed;
                        boom.acceleration = acceleration;
                        boom.knockback = m2Knockback;
                        boom.damageInterval = damageInterval;
                        boom.duration = duration;
                        boom.rotationSpeed = rotationSpeed;
                        boom.rightMult = -1;
                        if (forwardVector.x < 0) boom.rightMult = 1;
                        attackTimer = m2CD;

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
                        SpinSwordProjectile boom =
                            Instantiate(m2Projectile, transform.position, Quaternion.identity)
                            .GetComponent<SpinSwordProjectile>();

                        //Debug.Log(boom);
                        if (i == 0)
                        {
                            
                            boom.moveDir = forwardVector;
                        }
                        else
                        {
                            if (isFlippedSide)
                                boom.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                            else
                                boom.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);
                        }

                        boom.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                        boom.speed = speed;
                        boom.acceleration = acceleration;
                        boom.knockback = m2Knockback;
                        boom.damageInterval = damageInterval;
                        boom.duration = duration;
                        boom.rotationSpeed = rotationSpeed;
                        boom.rightMult = -1;
                        if (forwardVector.x < 0) boom.rightMult = 1;
                        attackTimer = m2CD;

                        isFlippedSide = !isFlippedSide;
                        flipCounter++;
                        if (flipCounter >= 2) currentSpread += angle;
                    }
                }
            }
        }
    }

    IEnumerator SwingAttack(Vector2 forward)
    {
        sprite.enabled = false;
        isSwingRight = !isSwingRight;
        float timer = 0.1f;

        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
        {
            SwordSwing swing =
                    Instantiate(m1Projectile, transform.position, Quaternion.identity, transform)
                    .GetComponentInChildren<SwordSwing>();

            HelperManager.instance.RotateTowardsDirection((GameManager.instance.cam.screenToWorld - (Vector2)GameManager.instance.playerMovement.transform.position).normalized, swing.transform.parent);
            swing.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
            swing.knockback = m1Knockback;
            //swing.moveDir = forward;

            if (isSwingRight) swing.swingAnimation.clip = swing.right;
            else swing.swingAnimation.clip = swing.left;
            swing.swingAnimation.Play();

            

            timer = 0.1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        this.transform.rotation = Quaternion.identity;
        sprite.enabled = true;
        yield return null;
    }
    
}
