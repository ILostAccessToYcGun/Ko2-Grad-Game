using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FlipDamage : MonoBehaviour
{
    public CircleCollider2D col;
    public float damage;
    public float knockback;
    public GameObject particle1;
    public GameObject particle2;
    public Vector3 spawnOffset;

    private void Start()
    {
        Invoke("DestroySelf", 0.2f);
        GameObject part1 = Instantiate(particle1, transform.position + spawnOffset, Quaternion.identity);
        part1.transform.localScale = transform.localScale * 0.5f;
        GameObject part2 = Instantiate(particle2, transform.position + spawnOffset, Quaternion.identity);
        part2.transform.localScale = transform.localScale * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, transform.position);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - transform.position).normalized * knockback;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
