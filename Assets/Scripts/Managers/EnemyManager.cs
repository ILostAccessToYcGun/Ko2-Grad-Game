using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] List<GameObject> TotalEnemyPool;
    [SerializeField] List<GameObject> CurrentEnemyPool;
    

    public GameObject EXPCrystal;

    [SerializeField] float spawnCD;
    float spawnTimer = 0.5f;
    public float baseSpawnTimer; //does not include difficulty scaling

    public Vector2 spawnRange;
    public int maxEnemies = 1000;
    public int enemyCount = 0;
    public List<GameObject> Enemies;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //bascially what we're going to do, we're going to spawn enemies outisde the range of teh camera and plus some
        //we are also goign to have an adjustable spawn rate

        if (enemyCount > maxEnemies) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnCD;
            //Spawn an enemy outside of the player's range

            for (int i = 0; i < 50; i++)
            {
                Vector2 pos = Random.insideUnitCircle * spawnRange.y;

                if (pos.magnitude > spawnRange.x) //valid spawn
                {
                    int rand = Random.Range(0, CurrentEnemyPool.Count);
                    Enemies.Add(Instantiate(CurrentEnemyPool[rand], GameManager.instance.playerMovement.transform.position + new Vector3(pos.x, pos.y, 0), Quaternion.identity));
                    break;
                }
            }
        }
    }

    public void SetSpawnCD(int difficulty)
    {
        Debug.Log("Setting new spawn cooldown");
        spawnCD = baseSpawnTimer * Mathf.Pow(0.75f, difficulty);
    }

    public void CleanUpEnemies()
    {
        EnemyBase[] enemyes = FindObjectsByType<EnemyBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (EnemyBase enemeh in enemyes)
        {
            Destroy(enemeh.gameObject);
        }
        spawnTimer = 0.5f;
    }

    
}
