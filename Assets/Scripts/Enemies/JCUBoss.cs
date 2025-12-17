using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JCUBoss : EnemyBase
{
    /* The JCU boss will have 3 attacks
     * Books Fall from the sky in random areas over 4 seconds
     * Syringes shoot beams in semi random directions, but generally towards the player
     * if the player is close, stomp the grounds
     
     
     */

    public Vector2 attackCooldownRange = new Vector3(5, 7);
    public float attackCooldown = 5.0f; //the time in between attack sequences
    public float slamKB = 3.0f;


    [Header("Components")]
    [SerializeField] Animation jcuAnimator;
    [SerializeField] AnimationClip spawn;
    [SerializeField] AnimationClip slamAttack;
    [SerializeField] GameObject spawnSlam;

    [SerializeField] int syringeCount;
    [SerializeField] GameObject syringeBlaster;
    [SerializeField] float syringeSpawnDist = 3.0f;

    [SerializeField] int bookCount = 30;
    [SerializeField] float bookRange = 20.0f;
    [SerializeField] List<GameObject> books;



    public void Start()
    {
        base.Start();
        attackCooldown = 6.0f;
    }

    void Update()
    {
        if (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        else //choose an attack
        {
            int rand = Random.Range(1, 4); //pick a number; 1, 2 or 3

            switch (rand)
            {
                case 1:
                    //SLAM
                    jcuAnimator.clip = slamAttack;
                    jcuAnimator.Play();

                    break;

                case 2:
                    //GHASTER BLASTER SYRINGE

                    StartCoroutine(Syringes());

                    break;

                case 3:
                    //BOOK RAIN
                    Debug.Log("book");
                    StartCoroutine(BookRain());
                    break;

            }
            RandomAttackCD();
        }

    }

    public void SpawnSlam()
    {
        JCUSlamDamage slam = Instantiate(spawnSlam).GetComponent<JCUSlamDamage>();
        slam.damage = ATK;
        slam.knockback = slamKB;
        RandomAttackCD();
    }

    public void RandomAttackCD()
    {
        attackCooldown = Random.Range(attackCooldownRange.x, attackCooldownRange.y);
    }

    IEnumerator Syringes()
    {
        float timer = 0.1f;
        Vector2 spawnOffset = Vector2.zero;

        for (int i = 0; i < syringeCount; i++)
        {
            if (i == syringeCount - 1)//the last one
            {
                timer = 0.1f;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }

                spawnOffset = (GameManager.instance.playerMovement.transform.position - transform.position).normalized * 10.0f;

                Debug.Log("Syringe");
                Syringe lastBeam =
                        Instantiate(syringeBlaster, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform)
                        .GetComponent<Syringe>();

                HelperManager.instance.RotateTowardsDirection((lastBeam.transform.position - (GameManager.instance.playerMovement.transform.position + ((Vector3)Random.insideUnitCircle * 0.1f))).normalized, lastBeam.transform);
                lastBeam.transform.localScale *= 2.0f;
                lastBeam.damage = ATK * 0.5f;
                lastBeam.beamDelay = 2.0f;
                lastBeam.totalTime = 5.0f;
            }
            else
            {
                spawnOffset = Random.insideUnitCircle.normalized * syringeSpawnDist;

                Debug.Log("Syringe");
                Syringe beam =
                        Instantiate(syringeBlaster, (Vector2)GameManager.instance.playerMovement.transform.position + spawnOffset, Quaternion.identity, transform)
                        .GetComponent<Syringe>();

                HelperManager.instance.RotateTowardsDirection((beam.transform.position - (GameManager.instance.playerMovement.transform.position + ((Vector3)Random.insideUnitCircle * 0.25f))).normalized, beam.transform);
                beam.damage = ATK * 0.5f;

                timer = 0.25f;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }
                yield return null;
            }
        }

        yield return null;
    }


    IEnumerator BookRain()
    {
        float timer = 0.1f;
        Vector2 spawnOffset = Vector2.zero;

        for (int i = 0; i < bookCount; i++)
        {
            spawnOffset = Random.insideUnitCircle * bookRange;

            Debug.Log("Book");
            BookSlam book =
                    Instantiate(books[Random.Range(0, 3)], (Vector2)GameManager.instance.playerMovement.transform.position + spawnOffset, Quaternion.identity, transform)
                    .GetComponent<BookSlam>();

            book.damage = ATK * 0.25f;

            timer = 0.1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

        yield return null;
    }

    private void OnDestroy()
    {
        TimeManager.instance.canTick = true;
        GameManager.instance.Win();

        UIManager.instance.UpdateTimeText("15:00");
    }
}
