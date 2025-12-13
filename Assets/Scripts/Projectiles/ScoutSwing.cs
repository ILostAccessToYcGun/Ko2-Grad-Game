using UnityEngine;

public class ScoutSwing : MonoBehaviour
{
    public Animation swingAnimation;
    public AnimationClip first;
    public AnimationClip second;
    public AnimationClip third;
    public SpriteRenderer sprite;
    public ScoutBlades parent;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;


    public void DestroySelf()
    {
        parent.bladeCounter--;
        Destroy(transform.parent.gameObject);
    }
}
