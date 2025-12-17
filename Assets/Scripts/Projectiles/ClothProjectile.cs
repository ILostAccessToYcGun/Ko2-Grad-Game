using System.Collections.Generic;
using UnityEngine;

public class ClothProjectile : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float speed;
    public float deceleration;
    public float knockback;
    public float duration = 5.0f;

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
            dmg.TakeDamage(damage, transform.position, 1);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moveDir * knockback;
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    
}
