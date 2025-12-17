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

    public GameObject part;

    private void Start()
    {
        if (moveDir.x > 0) sprite.flipX = true;
    }

    public void SlamHitBox()
    {
        col.enabled = true;
        CameraShake.instance.Shake(0.05f, 0.1f);
        Instantiate(part, transform.position + new Vector3(0.0f, -0.25f, 0.0f), Quaternion.identity);
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }

    
}
