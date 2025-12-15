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
    
    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            EnemyManager.instance.enemyCount--;
            EXPCrystal exp = Instantiate(EnemyManager.instance.EXPCrystal, transform.position, Quaternion.identity).GetComponent<EXPCrystal>();
            exp.EXP = EXP;
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerStats player = collision.GetComponent<PlayerStats>();
        if (player != null)
        {
            Debug.Log("why");
            if (ATKTimer > 0f) return;
            IDamage dmg = player.GetComponent<IDamage>();
            dmg.TakeDamage(ATK);
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

    public void TryBurn(float burnTime, float damage)
    {
        if (!isBurn) StartCoroutine(Burn(burnTime, damage));
    }

    IEnumerator Burn(float burnTime, float damage)
    {
        isBurn = true;
        float timer = 0.0f;
        float burnTimer = burnTime;
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
    }
}
