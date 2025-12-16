using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public void UpgradeATK()
    {
        GameManager.instance.playerStats.ATK += GameManager.instance.playerStats.baseATK * 0.1f;
        GameManager.instance.CloseUpgrade();
    }

    public void UpgradeATKSPD()
    {
        GameManager.instance.playerStats.ATKSPD += GameManager.instance.playerStats.baseATKSPD * 0.1f;
        GameManager.instance.CloseUpgrade();
    }

    public void UpgradeEXP()
    {
        GameManager.instance.playerStats.XPG += GameManager.instance.playerStats.baseXPG * 0.1f;
        GameManager.instance.playerStats.magnetRange += GameManager.instance.playerStats.baseMagnetRange * 0.25f;
        GameManager.instance.CloseUpgrade();
    }

    public void UpgradeHP()
    {
        float ratio = GameManager.instance.playerStats.currentHP / GameManager.instance.playerStats.maxHP;
        GameManager.instance.playerStats.maxHP += GameManager.instance.playerStats.baseHP * 0.1f;
        GameManager.instance.playerStats.currentHP = GameManager.instance.playerStats.maxHP * ratio;
        GameManager.instance.CloseUpgrade();
    }

    public void UpgradePRJ()
    {
        GameManager.instance.playerStats.PRJ += 1;
        GameManager.instance.CloseUpgrade();
    }

    public void UpgradeSPD()
    {
        GameManager.instance.playerStats.SPD += GameManager.instance.playerStats.baseSPD * 0.1f;
        GameManager.instance.CloseUpgrade();
    }


}
