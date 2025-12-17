using UnityEngine;

public class SwordSwingDamage : MonoBehaviour
{
    SwordSwing parent;
    private void Start()
    {
        parent = GetComponentInParent<SwordSwing>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(parent.damage, transform.position, 1);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = parent.moveDir * parent.knockback;
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        }
    }
}
