using NUnit.Framework.Constraints;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyBase : MonoBehaviour, IDamage
{
    [Header("Base Stats")]
    [SerializeField] int baseHP = 4;
    [SerializeField] int baseATK = 1;
    [SerializeField] float baseSPD = 1.0f;
    [SerializeField] float baseEXP = 5.0f;

    [SerializeField] float ATKCD = 1.00f;
    float ATKTimer = 0f;

    

    //The actual stats we will use for damage and calculations
    [Space]
    [Header("Stats")]
    public float HP;
    public float ATK;
    public float SPD;
    public float EXP;

    public PlayerMovement player;

    SpriteRenderer sprite;
    public Sprite frame1;
    public Sprite frame2;
    public float frameDelay;
    public bool canMove = true;
    public bool isBurn = false;
    public float burnTimer;

    public GameObject hitParticle;
    public GameObject deathParticle;
    public GameObject covidParticle;
    public GameObject currentCovid = null;

    Color originalColour;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        player = GameManager.instance.playerMovement;
        EnemyManager.instance.enemyCount++;
        float mult = TimeManager.instance.difficultyModifier;
        HP = baseHP * mult;
        ATK = baseATK * mult;
        SPD = baseSPD * mult;
        EXP = baseEXP * TimeManager.instance.expModifier;
        sprite = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(WalkAnimation());
        originalColour = sprite.color;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.instance.gameState != GameManager.States.Playing) return;
        PlayerStats player = collision.GetComponent<PlayerStats>();
        if (player != null)
        {
            //Debug.Log("why");
            if (ATKTimer > 0f) return;
            IDamage dmg = player.GetComponent<IDamage>();
            dmg.TakeDamage(ATK, transform.position);
            //TakeDamage(ATK);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        ATKTimer = ATKCD;
        while(ATKTimer > 0)
        {
            ATKTimer -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    protected void FacePlayer()
    {
        if (player.transform.position.x < transform.position.x && !sprite.flipX ||
            player.transform.position.x > transform.position.x && sprite.flipX)
        {
            sprite.flipX = !sprite.flipX;
        }
    }

    private void OnDestroy()
    {
        EnemyManager.instance.Enemies.Remove(this.gameObject);
    }

    IEnumerator WalkAnimation()
    {
        while (HP > 0)
        {
            if (sprite.sprite == frame1) sprite.sprite = frame2;
            else sprite.sprite = frame1;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    public void Stun(float stunTime)
    {
        canMove = false;
        Invoke("StunEnd", stunTime);
    }

    public void StunEnd()
    {
        canMove = true;
    }

    public void TryCovid(float burnTime, float damage)
    {
        if (!isBurn) StartCoroutine(Covid19(burnTime, damage));
        else burnTimer += burnTime;
    }

    IEnumerator Covid19(float burnTime, float damage)
    {
        currentCovid = Instantiate(covidParticle, transform);
        currentCovid.transform.localScale = sprite.transform.localScale;

        StartCoroutine(SycnCovid());
        isBurn = true;
        float timer = 0.0f;
        burnTimer = burnTime;
        while (burnTimer > 0)
        {
            burnTimer -= Time.deltaTime;
            timer = 1.0f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            TakeDamage(damage);
            yield return null;

        }
        yield return null;
        isBurn = false;
        Destroy(currentCovid);
    }

    IEnumerator SycnCovid()
    {
        while (currentCovid != null)
        {
            currentCovid.transform.position = transform.position;
            yield return null;
        }
    }

    public void TakeDamage(float damage, int evoId = 0)
    {
        TakeDamage(damage, Vector2.zero, evoId);
    }

    public void TakeDamage(float damage, Vector2 attackPos, int evoId = 0)
    {
        if (attackPos == Vector2.zero)
        {
            //random direction
            attackPos = Random.insideUnitCircle;
        }
        else
        {
            attackPos = attackPos - (Vector2)transform.position;
        }
            attackPos = -attackPos.normalized;

        HP -= damage;
        GameObject part1 = Instantiate(hitParticle, transform.position, Quaternion.identity);
        part1.transform.localScale = sprite.transform.localScale;
        HelperManager.instance.RotateTowardsDirection(attackPos, part1.transform);

        
        StartCoroutine(FlashRed());

        if (HP <= 0)
        {
            EnemyManager.instance.enemyCount--;
            EXPCrystal exp = Instantiate(EnemyManager.instance.EXPCrystal, transform.position, Quaternion.identity).GetComponent<EXPCrystal>();
            exp.EXP = EXP;

            if (evoId == 1)
            {
                GameManager.instance.playerStats.ProgressEvolution1();
            }
            else if (evoId == 2)
            {
                GameManager.instance.playerStats.ProgressEvolution2();
            }

            GameObject part2 = Instantiate(deathParticle, transform.position, Quaternion.identity);
            part2.transform.localScale = sprite.transform.localScale;
            //HelperManager.instance.RotateTowardsDirection(attackPos, part2.transform);

            GameManager.instance.playerStats.killCount++;
            CameraShake.instance.Shake(0.1f, 0.2f);
            Destroy(gameObject);
        }
        else
        {
            CameraShake.instance.Shake(0.05f, 0.075f);
        }
    }

    IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sprite.color = originalColour;
    }
}
