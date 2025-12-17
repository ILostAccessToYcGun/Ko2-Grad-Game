using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSwordProjectile : MonoBehaviour
{
    public SpriteRenderer sprite;
    public CircleCollider2D col;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float acceleration;
    public float knockback;
    public float speed;
    public float damageInterval;
    public float rotationSpeed;
    public float duration;
    public int rightMult;

    bool returning = false;

    void Update()
    {
        //damage intervals
        


        if (!returning)
        {
            if (speed > 0)
            {
                speed -= Time.deltaTime * acceleration;
                transform.position += (Vector3)moveDir * speed * Time.deltaTime;
            }
            else
            {
                returning = true;
            }
        }
        else
        {
            if (duration > 0)
            {
                speed = 0;
                duration -= Time.deltaTime;
            }
            else
            {
                speed += Time.deltaTime * acceleration * 2.0f;
                transform.position += (GameManager.instance.playerMovement.transform.position - transform.position).normalized * speed * Time.deltaTime;

                if (Vector3.Distance(GameManager.instance.playerMovement.transform.position, (Vector3)transform.position) < 0.5f)
                {
                    DestroySelf();
                }
            }
        }

        sprite.transform.Rotate(0.0f, 0.0f, rightMult * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, transform.position, 2);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - GameManager.instance.playerMovement.transform.position) * knockback;
            StartCoroutine(DamageFrame());
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageFrame()
    {
        col.enabled = false;
        float damageTimer = 0.0f;

        while (damageTimer < damageInterval)
        {
            damageTimer += Time.deltaTime;
            yield return null;
        }
        col.enabled = true;
        yield return null;
    }
}
