using UnityEngine;

public class ScoutSwingDamage : MonoBehaviour
{
    ScoutSwing parent;
    public GameObject part;
    private void Start()
    {
        parent = GetComponentInParent<ScoutSwing>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(parent.damage, transform.position);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = parent.moveDir * parent.knockback;
            Instantiate(part, transform.position, Quaternion.identity);
            if (parent.swingAnimation.clip == parent.third)
            {
                parent.parent.hitEnemy = true;
            }
        }
    }
}
