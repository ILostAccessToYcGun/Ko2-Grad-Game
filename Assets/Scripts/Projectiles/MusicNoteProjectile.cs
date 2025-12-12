using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MusicNoteProjectile : MonoBehaviour
{
    //fly in a wavy pattern, it lasts 10s
    //if it hits an enemy add it to the list of hit things, up to a max of 3
    //once you hit 3 targets
    public SpriteRenderer sprite;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float speed = 2.0f;
    public float oscillationSpeed = 5.0f;
    public float oscillationRange = 5.0f;
    public int pierce = 3;
    public float duration = 5.0f;
    float angle;

    private void Start()
    {
        Invoke("DestroySelf", duration);
        if (moveDir.x > 0) sprite.flipX = true;
    }

    public List<GameObject> hitTargets;
    void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;

        angle += Time.deltaTime * oscillationSpeed;

        transform.position += (Vector3)Vector2.Perpendicular(moveDir) * oscillationRange * Mathf.Sin(angle * Mathf.Deg2Rad) * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            if (!hitTargets.Contains(collision.gameObject))
            {
                hitTargets.Add(collision.gameObject);
                dmg.TakeDamage(damage);
            }

            if (hitTargets.Count >= pierce) DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
