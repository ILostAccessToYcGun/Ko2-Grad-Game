using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float elapsedTime;
    public float secondTimer;
    public int difficulty = 0;
    public float difficultyModifier = 1.0f;
    public float expModifier = 1.0f;
    public int seconds;
    public int minutes;

    public GameObject JCU;
    public bool canTick = true;
    public bool bossSpawned = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameManager.instance.gameState != GameManager.States.Playing) return;

        if (minutes == 14 && seconds == 59 && canTick && !bossSpawned)
        {
            bossSpawned = true;
            Instantiate(JCU, Vector3.zero, Quaternion.identity);
            canTick = false;
        }

        if (!canTick) return;

        //Timer Incrementing
        elapsedTime += Time.deltaTime;
        secondTimer += Time.deltaTime;

        if (elapsedTime >= 900.0f) GameManager.instance.Win();

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

            string TimeText = "";
            if (minutes < 10) TimeText += "0";
            TimeText += minutes;
            TimeText += ":";
            if (seconds < 10) TimeText += "0";
            TimeText += seconds;
            UIManager.instance.UpdateTimeText(TimeText);
        }
    }

    public void IncreaseDifficulty()
    {
        difficulty++;
        difficultyModifier *= 1.1f;
        expModifier *= 1.05f;
        // increase difficulty multipliticately by 10%
        //e.g 1.0f, 1.1f, 1.21, 1.331, 1.464, 1.611

        //the modifier will mainly be used for enemy stats, where the integer will be used for spawn rate
        EnemyManager.instance.SetSpawnCD(difficulty);
    }

    public void ResetDifficulty()
    {
        difficulty = 0;
        difficultyModifier = 1.0f;
        expModifier = 1.0f;
    }

    public void ResetTimers()
    {
        elapsedTime = 0;
        secondTimer = 0;
        seconds = 0;
        minutes = 0;
        bossSpawned = false;
        canTick = true;
    }
}
