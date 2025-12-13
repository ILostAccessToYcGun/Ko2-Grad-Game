using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CyclingFlail : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] SpriteRenderer sprite; 
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float m1Knockback;
    [SerializeField] float m1Speed;
    public List<GameObject> Flails;
    float m1HoldTimer;
    [SerializeField] Sprite handleSprite;
    [SerializeField] Sprite fullSprite;


    [Header("Mouse 2")]
    [SerializeField] float m2KickCD;
    [SerializeField] float m2DmgMult = 2.0f;
    [SerializeField] float m2DmgAmp = 2.0f;
    [SerializeField] float m2DmgTaken = 2.0f;
    [SerializeField] float m2Knockback;
    [SerializeField] float m2FlailFollowDist;
    [SerializeField] bool demonForm = false;
    [SerializeField] Color demonFormFlailColour;
    [SerializeField] Color demonFormChain;
    [SerializeField] Color normalChain;
    [SerializeField] Sprite kickSprite;
    [SerializeField] Sprite demonForm1;
    [SerializeField] Sprite demonForm2;
    [SerializeField] Sprite unDemonForm1;
    [SerializeField] Sprite unDemonForm2;


    //[SerializeField] float m2Knockback;
    [SerializeField] float m2Speed;
    [SerializeField] float m2Decceleration;
    //[SerializeField] float damageInterval;
    [SerializeField] float m2Duration;
    //[SerializeField] float angleSpread = 45.0f;
    //[SerializeField] float rotationSpeed = 300f;


    protected override void OnUpdate()
    {

        if (!demonForm)
        {
            if (m1HoldTimer > 5.0f)
            {
                foreach (GameObject flail in Flails)
                {
                    Destroy(flail);
                }
                Flails.Clear();
                sprite.sprite = fullSprite;
            }
            else
            {
                m1HoldTimer += Time.deltaTime;
            }
        }
        

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
                if (!demonForm)
                {
                    m1HoldTimer = 0.0f;

                    if (Flails.Count <= 0)
                    {
                        sprite.sprite = handleSprite;
                        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
                        {
                            FlailProjectile flail = Instantiate(m1Projectile, transform.position, Quaternion.identity).GetComponent<FlailProjectile>();
                            Flails.Add(flail.gameObject);
                            flail.knockback = m1Knockback;
                            flail.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                            flail.DmgAmp = m2DmgAmp;
                            flail.demonFollowDistance = m2FlailFollowDist;
                        }
                    }
                    else
                    {
                        foreach (GameObject flail in Flails)
                        {
                            flail.GetComponent<FlailProjectile>().Move((GameManager.instance.cam.screenToWorld - (Vector2)flail.transform.position).normalized * m1Speed);
                        }
                    }
                    attackTimer = m1CD;
                }
                else
                {
                    StartCoroutine(KickAttack(forwardVector));
                    attackTimer = m2KickCD;
                }
                
            }

            else if (m2.ReadValue<float>() > 0.0f)
            {
                if (Flails.Count <= 0)
                {
                    sprite.sprite = handleSprite;
                    for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
                    {
                        FlailProjectile flail = Instantiate(m1Projectile, transform.position + (Vector3)Random.insideUnitCircle * 0.5f, Quaternion.identity).GetComponent<FlailProjectile>();
                        Flails.Add(flail.gameObject);
                        flail.knockback = m1Knockback;
                        flail.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                        flail.DmgAmp = m2DmgAmp;
                        flail.demonFollowDistance = m2FlailFollowDist;
                    }
                }

                demonForm = !demonForm;
                if (demonForm)
                {
                    GameManager.instance.playerStats.sprite.sprite = demonForm1;
                    //second sprite will need to be here
                    GameManager.instance.playerStats.dmgTaken = m2DmgTaken;
                }
                else
                {
                    GameManager.instance.playerStats.sprite.sprite = unDemonForm1;
                    //second sprite will need to be here
                    GameManager.instance.playerStats.dmgTaken = GameManager.instance.playerStats.baseDmgTaken;
                }

                foreach (GameObject flail in Flails)
                {
                    if (demonForm)
                    {
                        flail.GetComponent<FlailProjectile>().SetDemonForm(demonForm, demonFormFlailColour, demonFormChain);
                    }
                    else
                        flail.GetComponent<FlailProjectile>().SetDemonForm(demonForm, Color.white, normalChain);
                }
                attackTimer = m2CD;
            }
        }
    }


    IEnumerator KickAttack(Vector2 forward)
    {
        float timer = 0.1f;
        //set sprite to kick here
        Sprite orig = GameManager.instance.playerStats.sprite.sprite;
        bool orient = GameManager.instance.playerStats.sprite.flipX;

        GameManager.instance.playerStats.sprite.sprite = kickSprite;

        if (forward.x < 0) GameManager.instance.playerStats.sprite.flipX = true;

        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
        {
            KickProjectile kick = Instantiate(m2Projectile, transform.position, Quaternion.identity).GetComponent<KickProjectile>();
            kick.damage = GameManager.instance.playerStats.ATK * m2DmgMult * m2DmgAmp;
            kick.knockback = m2Knockback;
            kick.deceleration = m2Decceleration;
            kick.duration = m2Duration;
            kick.speed = m2Speed;
            kick.moveDir = (GameManager.instance.cam.screenToWorld - (Vector2)GameManager.instance.playerMovement.transform.position).normalized;



            timer = 0.05f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

        GameManager.instance.playerStats.sprite.sprite = orig;
        GameManager.instance.playerStats.sprite.flipX = orient;

        yield return null;

    }
}
