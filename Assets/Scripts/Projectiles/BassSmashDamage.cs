using UnityEngine;

public class BassSmashDamage : MonoBehaviour
{
    public float damage;
    public float stunTime;
    public float knockback;
    public Vector2 moveDir;

    private void Start()
    {
        Invoke("DestroySelf", 0.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, transform.position);
            collision.GetComponent<EnemyBase>().Stun(stunTime);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moveDir * knockback;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
