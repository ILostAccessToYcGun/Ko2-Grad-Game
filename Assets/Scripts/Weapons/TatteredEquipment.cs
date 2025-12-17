using System.Collections;
using UnityEngine;

public class TatteredEquipment : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float startSpeed;
    [SerializeField] float deceleration;
    [SerializeField] float m1Knockback;
    [SerializeField] float duration;
    [SerializeField] float angleSpread = 30.0f;


    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.5f;
    [SerializeField] float m2Knockback;

    public GameObject part;
    ParticleSystem currentAfterImage;
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
                        ClothProjectile cloth =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity)
                            .GetComponent<ClothProjectile>();

                        if (isFlippedSide)
                            cloth.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                        else
                            cloth.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);

                        cloth.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        cloth.speed = startSpeed;
                        cloth.deceleration = deceleration;
                        cloth.knockback = m1Knockback;
                        cloth.duration = duration;
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
                        ClothProjectile cloth =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity)
                            .GetComponent<ClothProjectile>();

                        if (i == 0)
                        {
                            cloth.moveDir = forwardVector;
                        }
                        else
                        {
                            if (isFlippedSide)
                                cloth.moveDir = HelperManager.instance.RotateVector2(forwardVector, -currentSpread);
                            else
                                cloth.moveDir = HelperManager.instance.RotateVector2(forwardVector, currentSpread);
                        }

                        cloth.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        cloth.speed = startSpeed;
                        cloth.deceleration = deceleration;
                        cloth.knockback = m1Knockback;
                        cloth.duration = duration;
                        attackTimer = m1CD;

                        isFlippedSide = !isFlippedSide;
                        flipCounter++;
                        if (flipCounter >= 2) currentSpread += angle;
                    }
                }
            }
            
            else if (m2.ReadValue<float>() > 0.0f)
            {
                //the idea is to dash towards the mouse direction with a distance scaling with the player's speed
                attackTimer = m2CD;
                StartCoroutine(SashDash(forwardVector));
            }
        }
    }


    IEnumerator SashDash(Vector2 forward)
    {
        GameManager.instance.playerMovement.canMove = false;
        GameManager.instance.playerStats.canTakeDamage = false;
        Vector2 destination = (Vector2)GameManager.instance.playerMovement.transform.position + (forward * GameManager.instance.playerStats.SPD * 2.0f);
        float timer = 0.0f;
        int rightMult = -1;
        if (forward.x < 0) rightMult = 1;

        currentAfterImage = Instantiate(part, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        SpinProjectile spin =
                            Instantiate(m2Projectile, playerMovement.transform)
                            .GetComponent<SpinProjectile>();
        spin.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
        spin.knockback = m2Knockback;
        spin.rightMult = rightMult;
        spin.transform.localScale = new Vector3(spin.transform.localScale.x * -rightMult, spin.transform.localScale.y, spin.transform.localScale.z);
        spin.transform.localScale += new Vector3(GameManager.instance.playerStats.PRJ * -rightMult, GameManager.instance.playerStats.PRJ, GameManager.instance.playerStats.PRJ);
        
        
        var main = currentAfterImage.main;

        //Debug.Log(Vector3.Distance(GameManager.instance.playerMovement.transform.position, (Vector3)destination));
        while (Vector3.Distance(GameManager.instance.playerMovement.transform.position, (Vector3)destination) > 0.5f)
        {
            GameManager.instance.playerStats.sprite.gameObject.transform.Rotate(0.0f, 0.0f, rightMult * GameManager.instance.playerStats.SPD * Time.deltaTime * 300.0f);
            timer += Time.deltaTime;
            if (timer >= 1.5f) break;
            GameManager.instance.playerMovement.transform.position = Vector2.MoveTowards((Vector2)GameManager.instance.playerMovement.transform.position, destination, GameManager.instance.playerStats.SPD * Time.deltaTime * 5.0f);
            currentAfterImage.transform.position = playerMovement.transform.position;
            main.startRotation = GameManager.instance.playerStats.sprite.transform.eulerAngles.z * Mathf.Deg2Rad;
            yield return null;
        }
        Destroy(spin.gameObject);
        GameManager.instance.playerStats.sprite.gameObject.transform.rotation = Quaternion.identity;
        GameManager.instance.playerMovement.canMove = true;
        GameManager.instance.playerStats.canTakeDamage = true;
        yield return null;
    }
}
