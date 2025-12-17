using NUnit.Framework.Constraints;
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



    public void Start()
    {
        base.Start();
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
                    break;

                case 2:
                    //GHASTER BLASTER SYRINGE

                    break;

                case 3:
                    //BOOK RAIN
                    break;

            }
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
}
