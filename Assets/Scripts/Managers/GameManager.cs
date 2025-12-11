using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
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
    }

    public void Win()
    {
        Debug.Log("YOU WIN POG");
        GameManager.instance.gameState = GameManager.States.Win;
        Time.timeScale = 0;
        UIManager.instance.ToggleWin();
        UIManager.instance.ToggleHUD();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.instance.gameState = GameManager.States.Paused;
        UIManager.instance.TogglePause();
    }

    public void Unpause()
    {
        Time.timeScale = originalTimeScale;
        GameManager.instance.gameState = GameManager.States.Playing;
        UIManager.instance.TogglePause();
        UIManager.instance.ToggleHUD();

    }

    public void Play()
    {
        Debug.Log("huh");
        //reset everything
        GameManager.instance.gameState = GameManager.States.Playing;
        Time.timeScale = originalTimeScale;
        playerStats.ResetStats();
        TimeManager.instance.ResetDifficulty();
        TimeManager.instance.ResetTimers();
        GameManager.instance.playerMovement.ResetPosition();
        GameManager.instance.playerStats.ResetStats();
        EnemyManager.instance.CleanUpEnemies();
    }
}
