using NUnit.Framework.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
    public WeaponHolder weaponHolder;
    public GameObject weaponHand;
    public CameraTween cam;

    public enum States { Playing, Paused, Win, Loss}
    public States gameState = States.Playing;

    [SerializeField] float originalTimeScale;
    private void Awake()
    {
        instance = this;
        originalTimeScale = Time.timeScale;
        if (!playerMovement) playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (!playerStats) playerStats = FindFirstObjectByType<PlayerStats>();
        if (!weaponHolder) weaponHolder = FindFirstObjectByType<WeaponHolder>();
        if (!weaponHand) weaponHand = FindFirstObjectByType<IHoldWeapons>().gameObject;
        if (!cam) cam = FindFirstObjectByType<CameraTween>();
        Time.timeScale = 0;
        GameManager.instance.gameState = GameManager.States.Paused;
    }

    // Update is called once per frame
    public void Lose()
    {
        Debug.Log("YOU DED");
        GameManager.instance.gameState = GameManager.States.Loss;
        Time.timeScale = 0;
        UIManager.instance.ToggleLose();
        UIManager.instance.ToggleHUD();
        UIManager.instance.ShowFinalStats();
    }

    public void ForceLose()
    {
        Debug.Log("YOU DED");
        GameManager.instance.gameState = GameManager.States.Loss;
        Time.timeScale = 0;
        UIManager.instance.ToggleLose();
        UIManager.instance.TogglePause();
        UIManager.instance.ToggleConfirmation();
        UIManager.instance.ShowFinalStats();
    }

    public void Win()
    {
        Debug.Log("YOU WIN POG");
        GameManager.instance.gameState = GameManager.States.Win;
        Time.timeScale = 0;
        UIManager.instance.ToggleWin();
        UIManager.instance.ToggleHUD();
        UIManager.instance.ShowFinalStats();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.instance.gameState = GameManager.States.Paused;
        UIManager.instance.TogglePause();
        UIManager.instance.ToggleHUD();
    }

    public void Unpause()
    {
        Time.timeScale = originalTimeScale;
        GameManager.instance.gameState = GameManager.States.Playing;
        UIManager.instance.TogglePause();
        UIManager.instance.ToggleHUD();

    }

    public void Upgrade()
    {
        Time.timeScale = 0;
        GameManager.instance.gameState = GameManager.States.Paused;
        UIManager.instance.ToggleHUD();
        UIManager.instance.ToggleUpgrade();
    }

    public void CloseUpgrade()
    {
        Time.timeScale = originalTimeScale;
        GameManager.instance.gameState = GameManager.States.Playing;
        UIManager.instance.ToggleHUD();
        UIManager.instance.ToggleUpgrade();
    }

    public void Play()
    {
        //reset everything in preparation
        GameManager.instance.gameState = GameManager.States.Playing;
        Time.timeScale = originalTimeScale;
        playerStats.ResetStats();
        TimeManager.instance.ResetDifficulty();
        TimeManager.instance.ResetTimers();
        GameManager.instance.playerMovement.ResetPosition();
        GameManager.instance.playerMovement.ResetFrames();
        GameManager.instance.playerStats.ResetStats();
        EnemyManager.instance.CleanUpEnemies();
        EnemyManager.instance.CleanUpEXP();

        GameManager.instance.playerStats.SetCurrentWeapon(GameManager.instance.playerMovement.weaponToEquip);

        if (UIManager.instance.RhythmStylusUI.activeSelf) UIManager.instance.ToggleStylusUI();
        if (UIManager.instance.ScoutBladeUI.activeSelf) UIManager.instance.ToggleBladeUI();

        BassProjectile[] based = FindObjectsByType<BassProjectile>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (BassProjectile b in based)
        {
            Destroy(b.gameObject);
        }

        BladeFragment[] blades = FindObjectsByType<BladeFragment>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (BladeFragment b in blades)
        {
            Destroy(b.gameObject);
        }

        CircleProjectile[] circles = FindObjectsByType<CircleProjectile>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CircleProjectile c in circles)
        {
            Destroy(c.gameObject);
        }
    }
}
