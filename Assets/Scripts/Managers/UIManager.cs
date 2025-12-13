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
}
