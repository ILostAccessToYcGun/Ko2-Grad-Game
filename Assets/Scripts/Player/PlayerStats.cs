using UnityEngine;
using System.Collections;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public class PlayerStats : MonoBehaviour, IDamage
{
    [Header("Base Stats")]
    public int baseHP = 100;
    public int baseATK = 5;
    public float baseSPD = 3.0f;
    public float baseATKSPD = 1.0f; //only affects main weapon
    public int basePRJ = 1; //projectile count
    public float baseXPG = 1.0f; //xp gain
    public float baseLevelExp = 25.0f;
    public float baseMagnetRange = 2.5f; 
    public float baseDmgTaken = 1.0f; 
    public float baseSpeedMult = 1.0f; 

    //The actual stats we will use for damage and calculations
    [Space]
    [Header("Stats")]
    public float maxHP;
    public float currentHP;
    public float ATK;
    public float SPD;
    public float ATKSPD; //only affects main weapon
    public int PRJ; //projectile count
    public float XPG; //xp gain

    public float EXPForLevel;
    public float magnetRange; //xp suck
    public float dmgTaken; 
    public float speedMult;
    [Space]
    public float EXP;
    public int Level;
    [Space]
    [Header("Components")]
    public GameObject currentWeapon;
    public SpriteRenderer sprite;
    public GameObject hitParticle;
    public GameObject levelUpParticle;
    public GameObject evolveReadyParticle;
    public GameObject currentEvolve;

    public bool canTakeDamage;
    bool isRegen = false;

    [Header("Evolution")]
    public int Evolution1KillCount = 0;
    public int Evolution2KillCount = 0;
    public int Evolution1KillsRequired = 500;
    public int Evolution2KillsRequired = 500;
    public int killCount = 0;

    public bool evo1unlocked = false;
    public bool evo2unlocked = false;
    public bool evolved = false;

    Color originalColour;
    private void Start()
    {
        originalColour = sprite.color;
        ResetStats();
    }

    private void Update()
    {
        if (currentEvolve == null) return;
        if (!evolved)
        {
            currentEvolve.transform.position = transform.position + new Vector3(0.0f, -0.75f, 0.0f);
        }
        
    }

    public void ResetStats()
    {
        maxHP = baseHP;
        currentHP = maxHP;
        ATK = baseATK;
        SPD = baseSPD;
        ATKSPD = baseATKSPD;
        PRJ = basePRJ;
        XPG = baseXPG;
        EXP = 0.0f;
        Level = 1;
        EXPForLevel = baseLevelExp;
        magnetRange = baseMagnetRange;
        dmgTaken = baseDmgTaken;
        speedMult = baseSpeedMult;
        UIManager.instance.UpdateHPBar(currentHP, maxHP);
        UIManager.instance.UpdateEXPBar(EXP, EXPForLevel);

        Evolution1KillCount = Evolution1KillsRequired;
        Evolution2KillCount = Evolution2KillsRequired;

        evo1unlocked = false;
        evo2unlocked = false;
        evolved = false;

        sprite.color = originalColour;

        if (!isRegen)
            StartCoroutine(PassiveRegen());

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
        UIManager.instance.UpdateEXPBar(EXP, EXPForLevel);
        if (EXP >= EXPForLevel) LevelUp();
    }

    public void LevelUp()
    {
        GameObject part = Instantiate(levelUpParticle, transform.position, Quaternion.identity, transform);

        Level++;
        EXP -= EXPForLevel;
        EXPForLevel = baseLevelExp * Mathf.Pow(1.5f, Level - 1);
        UIManager.instance.UpdateEXPBar(EXP, EXPForLevel);
        GameManager.instance.Upgrade();

        EXPCrystal[] EXPs = FindObjectsByType<EXPCrystal>(FindObjectsSortMode.None);
        foreach (EXPCrystal exp in EXPs)
        {
            exp.UpdateRange();
        }

        if (EXP >= EXPForLevel)
        {
            Invoke("LevelUp", 1.0f);
        }
        
    }

    public void TakeHealing(float heal)
    {
        
        if (!canTakeDamage) return;
        currentHP += (heal);
        UIManager.instance.UpdateHPBar(currentHP, maxHP);

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }    

    public void SetCurrentWeapon(GameObject newWeapon)
    {
        if (currentWeapon != null) Destroy(currentWeapon);
        currentWeapon = Instantiate(newWeapon, GameManager.instance.weaponHolder.holder.gameObject.transform);
        UIManager.instance.UpdateWeaponSlot(currentWeapon.GetComponent<WeaponBase>().weaponSprite);
    }

    public void UpdateCurrentWeapon()
    {
        GameManager.instance.playerStats.SetCurrentWeapon(GameManager.instance.playerMovement.weaponToEquip);
    }

    IEnumerator PassiveRegen()
    {
        isRegen = true;
        float timer = 0.0f;
        while (currentHP > 0)
        {
            timer = 1.0f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            TakeHealing(maxHP * 0.005f);
            yield return null;

        }
        yield return null;
    }
    

    public void ProgressEvolution1()
    {
        Evolution1KillCount--;
        if (Evolution1KillCount <= 0 && !evo1unlocked)
        {
            evo1unlocked = true;
            UIManager.instance.ToggleEvolution1();
            UIManager.instance.EvolutionPopup();
            if (currentEvolve == null)
                currentEvolve = Instantiate(evolveReadyParticle, transform.position, Quaternion.identity, transform);
        }
    }

    public void ProgressEvolution2()
    {
        Evolution2KillCount--;
        if (Evolution2KillCount <= 0 && !evo2unlocked)
        {
            evo2unlocked = true;
            UIManager.instance.ToggleEvolution2();
            UIManager.instance.EvolutionPopup();
            if (currentEvolve == null)
                currentEvolve = Instantiate(evolveReadyParticle, transform.position, Quaternion.identity, transform);
        }
    }

    public void TakeDamage(float damage, int evoId = 0)
    {
        TakeDamage(damage, Vector2.zero, evoId);
    }

    public void TakeDamage(float damage, Vector2 attackPos, int evoId = 0)
    {
        if (!canTakeDamage) return;

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

        currentHP -= (damage * dmgTaken);
        UIManager.instance.UpdateHPBar(currentHP, maxHP);

        GameObject part = Instantiate(hitParticle, transform.position, Quaternion.identity);
        HelperManager.instance.RotateTowardsDirection(attackPos, part.transform);

        CameraShake.instance.Shake(0.1f, 0.25f);
        StartCoroutine(FlashRed());
        AudioManager.instance.Play("Hit", 0.75f, 1.25f);

        if (currentHP <= 0)
        {
            AudioManager.instance.Play("Death", 0.8f, 1.2f);
            GameManager.instance.Lose();
            isRegen = false;
        }
    }

    public void ClearCanEvolveStatus()
    {
        Destroy(currentEvolve);
        currentEvolve = null;
        evolved = true;
        AudioManager.instance.Play("Evolve", 0.9f, 1.1f);
    }


    IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sprite.color = originalColour;
    }
}
