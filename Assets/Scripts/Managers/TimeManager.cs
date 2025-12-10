using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float elapsedTime;
    public float secondTimer;
    public int difficulty = 0;
    public float difficultyModifier = 1.0f;
    public int seconds;
    public int minutes;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameManager.instance.gameState != GameManager.States.Playing) return;
        //Timer Incrementing
        elapsedTime += Time.deltaTime;
        secondTimer += Time.deltaTime;

        //Timer Values for UI & scaling
        if (secondTimer >= 1.0f)
        {
            secondTimer -= 1.0f;
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                IncreaseDifficulty();
            }
        }
    }

    public void IncreaseDifficulty()
    {
        difficulty++;
        difficultyModifier *= 1.1f;
        // increase difficulty multipliticately by 10%
        //e.g 1.0f, 1.1f, 1.21, 1.331, 1.464, 1.611

        //the modifier will mainly be used for enemy stats, where the integer will be used for spawn rate
        EnemyManager.instance.SetSpawnCD(difficulty);
    }
}
