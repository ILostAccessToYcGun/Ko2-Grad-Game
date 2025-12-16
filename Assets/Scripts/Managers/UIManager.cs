using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject WeaponSelect;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject Confirmation;
    [SerializeField] GameObject Upgrade;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;
    [SerializeField] GameObject HUD;

    [SerializeField] Image HPBar;
    [SerializeField] Image EXPBar;
    [SerializeField] Image WeaponSlot;
    [SerializeField] TextMeshProUGUI Time;

    [Header("Upgrades")]
    [SerializeField] TextMeshProUGUI Stats;
    [SerializeField] GameObject HP_Upgrade;
    [SerializeField] GameObject ATK_Upgrade;
    [SerializeField] GameObject SPD_Upgrade;
    [SerializeField] GameObject ATKSPD_Upgrade;
    [SerializeField] GameObject EXP_Upgrade;
    [SerializeField] GameObject PRJ_Upgrade;
    [SerializeField] GameObject Upgrade1;
    [SerializeField] GameObject Upgrade2;
    [SerializeField] GameObject Upgrade3;
    [SerializeField] GameObject Upgrade4;
    [SerializeField] List<GameObject> UpgradePool;


    [Header("ScoutBlades")]
    [SerializeField] GameObject ScoutBladeUI;
    [SerializeField] Image Blade1;
    [SerializeField] Image Blade2;
    [SerializeField] Image Blade3;
    [SerializeField] Image Blade4;
    [SerializeField] Image Blade5;

    [Header("RhythmStylus")]
    [SerializeField] GameObject RhythmStylusUI;
    [SerializeField] Image RhythmStylusFill;
    [SerializeField] Image ExplosionFill;

    [Header("Evolution")]
    [SerializeField] GameObject evolution1;
    [SerializeField] Image evo1Icon;
    [SerializeField] TextMeshProUGUI evo1Name;
    [SerializeField] GameObject evolution2;
    [SerializeField] Image evo2Icon;
    [SerializeField] TextMeshProUGUI evo2Name;

    [SerializeField] GameObject evolution1UI;
    [SerializeField] GameObject evolution1Lock;
    [SerializeField] TextMeshProUGUI evolution1LockText;
    [SerializeField] GameObject evolution2UI;
    [SerializeField] GameObject evolution2Lock;
    [SerializeField] TextMeshProUGUI evolution2LockText;

    [Header("Final Stats")]
    [SerializeField] TextMeshProUGUI finalStats;
    [SerializeField] TextMeshProUGUI timeSurvived;
    [SerializeField] TextMeshProUGUI kills;
    [SerializeField] TextMeshProUGUI weapon;
    [SerializeField] Image weaponIcon;

    [SerializeField] TextMeshProUGUI finalStats2;
    [SerializeField] TextMeshProUGUI timeSurvived2;
    [SerializeField] TextMeshProUGUI kills2;
    [SerializeField] TextMeshProUGUI weapon2;
    [SerializeField] Image weaponIcon2;

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void ToggleMainMenu()
    {
        if (MainMenu != null) MainMenu.SetActive(!MainMenu.activeSelf);
    }

    public void ToggleWeaponSelect()
    {
        if (WeaponSelect != null) WeaponSelect.SetActive(!WeaponSelect.activeSelf);
    }

    public void TogglePause()
    {
        if (Pause != null) Pause.SetActive(!Pause.activeSelf);
    }

    public void ToggleConfirmation()
    {
        if (Confirmation != null) Confirmation.SetActive(!Confirmation.activeSelf);
    }

    public void ToggleUpgrade()
    {
        if (Upgrade == null) return;
        if (Upgrade.activeSelf == true)
        {
            Upgrade.SetActive(false);
        }
        else
        {
            UpdateStatDetails();
            PopulateUpgradePool();
            //ADD RANDOM UPGRADES HERE
            int random = 0;
            GameObject current = UpgradePool[random];
            Upgrade.SetActive(true);

            // slot 1
            random = Random.Range(0, UpgradePool.Count);
            current = UpgradePool[random];
            Instantiate(UpgradePool[random], Upgrade1.transform);
            while (UpgradePool.Contains(current))
            {
                UpgradePool.Remove(current);
            }
            // slot 2
            random = Random.Range(0, UpgradePool.Count);
            current = UpgradePool[random];
            Instantiate(UpgradePool[random], Upgrade2.transform);
            while (UpgradePool.Contains(current))
            {
                UpgradePool.Remove(current);
            }
            // slot 3
            random = Random.Range(0, UpgradePool.Count);
            current = UpgradePool[random];
            Instantiate(UpgradePool[random], Upgrade3.transform);
            while (UpgradePool.Contains(current))
            {
                UpgradePool.Remove(current);
            }
            // slot 4
            random = Random.Range(0, UpgradePool.Count);
            current = UpgradePool[random];
            Instantiate(UpgradePool[random], Upgrade4.transform);
            while (UpgradePool.Contains(current))
            {
                UpgradePool.Remove(current);
            }
        }
    }

    void PopulateUpgradePool()
    {
        UpgradePool.Clear();
        UpgradePool.Add(HP_Upgrade);
        UpgradePool.Add(HP_Upgrade);
        UpgradePool.Add(ATK_Upgrade);
        UpgradePool.Add(ATK_Upgrade);
        UpgradePool.Add(SPD_Upgrade);
        UpgradePool.Add(SPD_Upgrade);
        UpgradePool.Add(ATKSPD_Upgrade);
        UpgradePool.Add(ATKSPD_Upgrade);
        UpgradePool.Add(EXP_Upgrade);
        UpgradePool.Add(EXP_Upgrade);
        UpgradePool.Add(PRJ_Upgrade);
    }

    void UpdateStatDetails()
    {
        Stats.text =
            "HP: " + GameManager.instance.playerStats.maxHP + "\n" +
            "ATK: " + GameManager.instance.playerStats.ATK + "\n" +
            "SPD: " + GameManager.instance.playerStats.SPD + "\n" +
            "ATKSPD: " + GameManager.instance.playerStats.ATKSPD + "\n" +
            "EXP: " + GameManager.instance.playerStats.XPG + "\n" +
            "PRJ: " + GameManager.instance.playerStats.PRJ + "\n";
    }

    public void ToggleWin()
    {
        if (Win != null) Win.SetActive(!Win.activeSelf);
    }
    public void ToggleLose()
    {
        if (Lose != null) Lose.SetActive(!Lose.activeSelf);
    }
    public void ToggleHUD()
    {
        if (HUD != null) HUD.SetActive(!HUD.activeSelf);
    }

    public void UpdateHPBar(float value, float max)
    {
        HPBar.fillAmount = value / max;
    }

    public void UpdateEXPBar(float value, float max)
    {
        EXPBar.fillAmount = value / max;
    }

    public void UpdateWeaponSlot(Sprite sprite)
    {
        WeaponSlot.sprite = sprite;
    }

    public void UpdateTimeText(string text)
    {
        Time.text = text;
    }

    public void Play()
    {
        Debug.Log("Choose ur weapon now");
        ToggleWeaponSelect();
        ToggleMainMenu();
    }

    public void Extras()
    {
        Debug.Log("gj on graduating med school frfr");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnFromLose()
    {
        Debug.Log("ur done now bro");
        //below is temp i think
        ToggleMainMenu();
        ToggleLose();
        //GameManager.instance.Pause();
        GameManager.instance.playerMovement.weaponToEquip = null;
    }

    public void ReturnFromWin()
    {
        Debug.Log("ur done now bro");
        //below is temp i think
        ToggleMainMenu();
        ToggleWin();
        //GameManager.instance.Pause();
        GameManager.instance.playerMovement.weaponToEquip = null;
    }

    public void LockIn()
    {
        //below is temp i think
        if (GameManager.instance.playerMovement.weaponToEquip == null) return;
        ToggleWeaponSelect();
        ToggleHUD();
        GameManager.instance.Play();
    }


    public void ToggleBladeUI()
    {
        if (ScoutBladeUI != null) ScoutBladeUI.SetActive(!ScoutBladeUI.activeSelf);
    }

    public void UpdateBladeCount(int blades)
    {
        switch (blades)
        {
            case 0:
                Blade1.enabled = false;
                Blade2.enabled = false;
                Blade3.enabled = false;
                Blade4.enabled = false;
                Blade5.enabled = false;
                break;

            case 1:
                Blade1.enabled = true;
                Blade2.enabled = false;
                Blade3.enabled = false;
                Blade4.enabled = false;
                Blade5.enabled = false;
                break;

            case 2:
                Blade1.enabled = true;
                Blade2.enabled = true;
                Blade3.enabled = false;
                Blade4.enabled = false;
                Blade5.enabled = false;
                break;

            case 3:
                Blade1.enabled = true;
                Blade2.enabled = true;
                Blade3.enabled = true;
                Blade4.enabled = false;
                Blade5.enabled = false;
                break;

            case 4:
                Blade1.enabled = true;
                Blade2.enabled = true;
                Blade3.enabled = true;
                Blade4.enabled = true;
                Blade5.enabled = false;
                break;

            case 5:
                Blade1.enabled = true;
                Blade2.enabled = true;
                Blade3.enabled = true;
                Blade4.enabled = true;
                Blade5.enabled = true;
                break;
        }
    }

    public void ToggleStylusUI()
    {
        if (RhythmStylusUI != null) RhythmStylusUI.SetActive(!RhythmStylusUI.activeSelf);
    }

    public void UpdateStylusReserve(float ratio)
    {
        RhythmStylusFill.fillAmount = ratio;
    }

    public void UpdateExplosionReserve(float ratio)
    {
        ExplosionFill.fillAmount = ratio;
    }

    public void ChooseWeapon(GameObject weapon)
    {
        GameManager.instance.playerMovement.weaponToEquip = weapon;
    }
    public void SetEvolution1(GameObject weapon)
    {
        evolution1 = weapon;
        evo1Icon.sprite = weapon.GetComponent<WeaponBase>().weaponSprite;
        evo1Name.text = weapon.GetComponent<WeaponBase>().weaponName;
    }
    public void SetEvolution2(GameObject weapon)
    {
        evolution2 = weapon;
        evo2Icon.sprite = weapon.GetComponent<WeaponBase>().weaponSprite;
        evo2Name.text = weapon.GetComponent<WeaponBase>().weaponName;
    }

    public void ChooseEvolution1()
    {
        GameManager.instance.playerMovement.weaponToEquip = evolution1;
        DisableEvolutions();
    }
    public void ChooseEvolution2()
    {
        GameManager.instance.playerMovement.weaponToEquip = evolution2;
        DisableEvolutions();
    }

    public void ToggleEvolution1()
    {
        if (evolution1UI != null) evolution1UI.SetActive(!evolution1UI.activeSelf);
        if (evolution1Lock != null) evolution1Lock.SetActive(!evolution1Lock.activeSelf);
    }

    public void ToggleEvolution2()
    {
        if (evolution2UI != null) evolution2UI.SetActive(!evolution2UI.activeSelf);
        if (evolution2Lock != null) evolution2Lock.SetActive(!evolution2Lock.activeSelf);
    }

    public void DisableEvolutions()
    {
        evolution1UI.SetActive(false);
        evolution1Lock.SetActive(true);

        evolution2UI.SetActive(false);
        evolution2Lock.SetActive(true);

        evolution1LockText.text = "Weapon has Evolved, Cannot Evolve again";
        evolution2LockText.text = "Weapon has Evolved, Cannot Evolve again";

        GameManager.instance.playerStats.Evolution1KillCount = -1;
        GameManager.instance.playerStats.Evolution2KillCount = -1;

        GameManager.instance.playerStats.UpdateCurrentWeapon();
    }

    public void ShowFinalStats()
    {
        finalStats.text =
            "HP: " + GameManager.instance.playerStats.maxHP + "\n" +
            "ATK: " + GameManager.instance.playerStats.ATK + "\n" +
            "SPD: " + GameManager.instance.playerStats.SPD + "\n" +
            "ATKSPD: " + GameManager.instance.playerStats.ATKSPD + "\n" +
            "EXP: " + GameManager.instance.playerStats.XPG + "\n" +
            "PRJ: " + GameManager.instance.playerStats.PRJ;

        timeSurvived.text = "Time Survived: " + Time.text;
        kills.text = "Kills: " + GameManager.instance.playerStats.killCount;
        weapon.text = "Weapon: " + GameManager.instance.playerMovement.weaponToEquip.GetComponent<WeaponBase>().weaponName;
        weaponIcon.sprite = GameManager.instance.playerMovement.weaponToEquip.GetComponent<WeaponBase>().weaponSprite;

        finalStats2.text =
            "HP: " + GameManager.instance.playerStats.maxHP + "\n" +
            "ATK: " + GameManager.instance.playerStats.ATK + "\n" +
            "SPD: " + GameManager.instance.playerStats.SPD + "\n" +
            "ATKSPD: " + GameManager.instance.playerStats.ATKSPD + "\n" +
            "EXP: " + GameManager.instance.playerStats.XPG + "\n" +
            "PRJ: " + GameManager.instance.playerStats.PRJ;

        timeSurvived2.text = "Time Survived: " + Time.text;
        kills2.text = "Kills: " + GameManager.instance.playerStats.killCount;
        weapon2.text = "Weapon: " + GameManager.instance.playerMovement.weaponToEquip.GetComponent<WeaponBase>().weaponName;
        weaponIcon2.sprite = GameManager.instance.playerMovement.weaponToEquip.GetComponent<WeaponBase>().weaponSprite;
    }
}
