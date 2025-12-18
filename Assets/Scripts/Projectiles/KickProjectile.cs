using System.Collections.Generic;
using UnityEngine;

public class KickProjectile : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float speed;
    public float deceleration;
    public float knockback;
    public float duration = 0.5f;

    private void Start()
    {
        HelperManager.instance.RotateTowardsDirection(moveDir, transform);
        Invoke("DestroySelf", duration);
    }
    void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;

        if (speed > 0) speed -= Time.deltaTime * deceleration;
        else speed = 0;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            //kick logic here
            dmg.TakeDamage(damage, transform.position);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moveDir * knockback;
        }

        FlailProjectile flail = collision.gameObject.GetComponent<FlailProjectile>();
        if (flail != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moveDir * knockback * 4.0f;
            //StartCoroutine(flail.Kicked());
            flail.Kicked();
            AudioManager.instance.Play("FlailKick", 0.75f, 1.25f);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    
}
