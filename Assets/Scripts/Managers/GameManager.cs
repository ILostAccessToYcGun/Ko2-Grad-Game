using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;

    public enum States { Playing, Paused, Win, Loss}
    public States gameState = States.Playing;

    private void Awake()
    {
        instance = this;
        if (!playerMovement) playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (!playerStats) playerStats = FindFirstObjectByType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
