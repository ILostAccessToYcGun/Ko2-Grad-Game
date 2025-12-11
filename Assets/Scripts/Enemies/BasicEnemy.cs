using UnityEngine;

public class BasicEnemy : EnemyBase
{   
    //WALK TOWARDS PLAYER
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, SPD * Time.deltaTime);
        FacePlayer();
    }
}
