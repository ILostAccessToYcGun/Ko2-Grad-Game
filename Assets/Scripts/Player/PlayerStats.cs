using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamage
{
    [Header("Base Stats")]
    [SerializeField] int baseHP = 100;
    [SerializeField] int baseATK = 5;
    [SerializeField] float baseSPD = 3.0f;
    [SerializeField] float baseATKSPD = 10.0f; //only affects main weapon
    [SerializeField] int basePRJ = 1; //projectile count
    [SerializeField] float baseXP = 1.0f; //xp gain

    //The actual stats we will use for damage and calculations
    [Space]
    [Header("Stats")]
    public float HP;
    public float ATK;
    public float SPD;
    public float ATKSPD; //only affects main weapon
    public int PRJ; //projectile count
    public float XP; //xp gain

    private void Start()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        HP = baseHP;
        ATK = baseATK;
        SPD = baseSPD;
        ATKSPD = baseATKSPD;
        PRJ = basePRJ;
        XP = baseXP;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Debug.Log("YOU DED");
            GameManager.instance.gameState = GameManager.States.Loss;
        }
    }
}
