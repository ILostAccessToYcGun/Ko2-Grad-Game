using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject Upgrade;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;

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

    public void Play()
    {
        Debug.Log("Choose ur weapon now");
        //below is temp i think
        ToggleMainMenu();
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
