using System.Collections;
using UnityEngine;

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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerStats player = collision.GetComponent<PlayerStats>();
        if (player != null)
        {
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
}
