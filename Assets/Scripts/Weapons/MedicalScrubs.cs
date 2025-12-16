using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class MedicalScrubs : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float m1Speed;
    [SerializeField] float deceleration;
    [SerializeField] float maxSize; //distance
    [SerializeField] float m1Knockback;
    [SerializeField] float endDuration;


    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.5f;
    [SerializeField] Vector2 m2SpeedRange;
    [SerializeField] Vector2 durationRange;
    [SerializeField] float DOTInterval;
    [SerializeField] Vector2 sizeGainRange;
    bool isEmitting = false;
    float emitTimer = 0.0f;
    float emitInterval = 1.0f;
    [Space]
    [SerializeField] Sprite scrub1;
    [SerializeField] Sprite scrub2;
    new public void Start()
    {
        base.Start();
        GameManager.instance.playerStats.sprite.sprite = scrub1;
        GameManager.instance.playerMovement.currentFrame1 = scrub1;
        GameManager.instance.playerMovement.currentFrame2 = scrub2;
    }

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
                StartCoroutine(SocialDistancing());
                attackTimer = m1CD;
                //SOCIAL DISTANCING
            }
            
            else if (m2.ReadValue<float>() > 0.0f)
            {
                //M2 attack
                //anesthetic trail

                isEmitting = !isEmitting;
                if (isEmitting) StartCoroutine(Anesthetic());
                attackTimer = m2CD;
                
            }
        }
    }

    IEnumerator SocialDistancing()
    {
        float timer = 0.1f;
        Vector2 randRotation;
        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
        {
            randRotation = Random.insideUnitCircle;

            MaskProjectile mask =
                    Instantiate(m1Projectile, GameManager.instance.playerMovement.transform.position, Quaternion.identity, GameManager.instance.playerMovement.transform)
                    .GetComponent<MaskProjectile>();

            HelperManager.instance.RotateTowardsDirection(randRotation, mask.transform);
            mask.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
            mask.speed = m1Speed;
            mask.deceleration = deceleration;
            mask.maxSize = maxSize;
            mask.knockback = m1Knockback;
            mask.duration = endDuration;

            timer = 0.25f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator Anesthetic()
    {
        
        float timer = 0.1f;

        while (isEmitting)
        {

            for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
            {
                AnestheticProjectile anesthetic =
                        Instantiate(m2Projectile, GameManager.instance.playerMovement.transform.position, Quaternion.identity)
                        .GetComponent<AnestheticProjectile>();

                anesthetic.moveDir = Random.insideUnitCircle.normalized;
                anesthetic.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                anesthetic.speed = Random.Range(m2SpeedRange.x, m2SpeedRange.y);
                anesthetic.duration = Random.Range(durationRange.x, durationRange.y); ;
                anesthetic.DOTInterval = DOTInterval;
                anesthetic.sizeGain = Random.Range(sizeGainRange.x, sizeGainRange.y); ;

                timer = 0.05f;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }
                yield return null;
            }

            emitTimer = emitInterval;
            while (emitTimer > 0)
            {
                emitTimer -= Time.deltaTime * GameManager.instance.playerStats.ATKSPD;
                yield return null;
            }
            yield return null;
        }

        
    }

}
