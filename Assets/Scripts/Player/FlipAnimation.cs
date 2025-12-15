using UnityEngine;

public class FlipAnimation : MonoBehaviour
{
    public Animation flipAnimation;
    public AnimationClip clock;
    public AnimationClip anti;
    public ArtisticSoles parent;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;


    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }

    public void FinishFlip()
    {
        FlipDamage flipDmg = Instantiate(parent.m1Projectile, GameManager.instance.playerMovement.transform.position, Quaternion.identity).GetComponent<FlipDamage>();
        flipDmg.transform.localScale *= 1 + (parent.flipCounter * 0.5f);
        flipDmg.damage = GameManager.instance.playerStats.ATK * parent.m1DmgMult * (1 + (parent.flipCounter * 0.5f));
        flipDmg.knockback = parent.m1Knockback;

        GameManager.instance.playerMovement.canMove = true;
        GameManager.instance.playerStats.canTakeDamage = true;
        parent.canFlip = true; 
        parent.checkConsecutive = true;
        parent.consecutiveTimer = parent.consecutiveTimeFrame;

        if (parent.flipCounter >= GameManager.instance.playerStats.PRJ) parent.flipCounter = 0;
    }
}
