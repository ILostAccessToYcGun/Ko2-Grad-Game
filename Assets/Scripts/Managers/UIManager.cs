using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject Upgrade;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;
    [SerializeField] GameObject HUD;

    [SerializeField] Image HPBar;
    [SerializeField] Image EXPBar;
    [SerializeField] Image WeaponSlot;
    [SerializeField] TextMeshProUGUI Time;

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

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void ToggleMainMenu()
    {
        if (MainMenu != null) MainMenu.SetActive(!MainMenu.activeSelf);
    }

    public void TogglePause()
    {
        if (Pause != null) Pause.SetActive(!Pause.activeSelf);
    }

    public void ToggleUpgrade()
    {
        if (Upgrade != null) Upgrade.SetActive(!Upgrade.activeSelf);
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
        //below is temp i think
        ToggleMainMenu();
        ToggleHUD();
        GameManager.instance.Play();
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
        GameManager.instance.Pause();
    }

    public void ReturnFromWin()
    {
        Debug.Log("ur done now bro");
        //below is temp i think
        ToggleMainMenu();
        ToggleWin();
        GameManager.instance.Pause();
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
}
