using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class FlipDamage : MonoBehaviour
{
    public CircleCollider2D col;
    public float damage;
    public float knockback;

    private void Start()
    {
        Invoke("DestroySelf", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - transform.position).normalized * knockback;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
