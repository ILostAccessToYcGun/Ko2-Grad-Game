using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public Animation swingAnimation;
    public AnimationClip right;
    public AnimationClip left;
    public SpriteRenderer sprite;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;


    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetAnimRight()
    {
        swingAnimation.clip = right;
    }

    public void SetAnimLeft()
    {
        swingAnimation.clip = left;
    }
}
