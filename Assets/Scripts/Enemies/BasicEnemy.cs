using UnityEngine;

public class BasicEnemy : EnemyBase
{   
    //WALK TOWARDS PLAYER
    void Update()
    {
        if (!canMove) return;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, SPD * Time.deltaTime);
        FacePlayer();

        if (Vector2.Distance(transform.position, player.transform.position) < 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + Random.insideUnitSphere, SPD * Time.deltaTime);
        }
    }
}
