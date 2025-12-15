using System.Collections.Generic;
using UnityEngine;

public class MaskProjectile : MonoBehaviour
{
    //public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float speed;
    public float deceleration;
    public float maxSize;
    public float knockback;
    public float duration = 5.0f;
    public float burnTime = 5.0f;

    bool isDestroying = false;

    void Update()
    {
        transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);

        if (transform.localScale.x >= maxSize)
        {
            if (speed > 0) speed -= Time.deltaTime * deceleration;
            else speed = 0;
        }

        if (speed == 0 && !isDestroying)
        {
            isDestroying = true;
            Invoke("DestroySelf", duration);
        }
            

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - transform.position).normalized * knockback;
            collision.gameObject.GetComponent<EnemyBase>().TryBurn(burnTime, damage * 0.2f);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    
}
