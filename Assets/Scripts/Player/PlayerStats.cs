using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamage
{
    [Header("Base Stats")]
    [SerializeField] int baseHP = 100;
    [SerializeField] int baseATK = 5;
    [SerializeField] float baseSPD = 3.0f;
    [SerializeField] float baseATKSPD = 10.0f; //only affects main weapon
    [SerializeField] int basePRJ = 1; //projectile count
    [SerializeField] float baseXPG = 1.0f; //xp gain
    [SerializeField] float baseLevelExp = 25.0f; 
    [SerializeField] float baseMagnetRange = 2.5f; 

    //The actual stats we will use for damage and calculations
    [Space]
    [Header("Stats")]
    public float HP;
    public float ATK;
    public float SPD;
    public float ATKSPD; //only affects main weapon
    public int PRJ; //projectile count
    public float XPG; //xp gain

    public float EXPForLevel;
    public float magnetRange; //xp suck
    [Space]
    public float EXP;
    public int Level;

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
        XPG = baseXPG;
        EXP = 0.0f;
        Level = 1;
        EXPForLevel = baseLevelExp;
        magnetRange = baseMagnetRange;

        //temp
        EXPCrystal[] EXPs = FindObjectsByType<EXPCrystal>(FindObjectsSortMode.None);

        foreach (EXPCrystal exp in EXPs)
        {
            exp.UpdateRange();
        }
    }

    public void GainEXP(float amount)
    {
        Debug.Log(" + " + amount * XPG + " EXP!");
        EXP += amount * XPG;
        if (EXP >= EXPForLevel) LevelUp();
    }

    public void LevelUp()
    {
        Level++;
        EXP -= EXPForLevel;
        EXPForLevel = baseLevelExp * Mathf.Pow(1.2f, Level - 1);
        if (EXP >= EXPForLevel) LevelUp();
        //will need an interrupt here for upgrade UI
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            GameManager.instance.Lose();
        }
    }
}
