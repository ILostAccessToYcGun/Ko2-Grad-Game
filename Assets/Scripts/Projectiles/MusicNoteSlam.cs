using System.Collections.Generic;
using UnityEngine;

public class MusicNoteSlam : MonoBehaviour
{
    //
    public Animation slamAnim;
    public CircleCollider2D col;
    public SpriteRenderer sprite;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;

    private void Start()
    {
        if (moveDir.x > 0) sprite.flipX = true;
    }

    public void SlamHitBox()
    {
        col.enabled = true;
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
    }
}
