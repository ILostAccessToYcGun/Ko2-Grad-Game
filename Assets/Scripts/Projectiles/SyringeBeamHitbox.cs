using UnityEngine;

public class SyringeBeamHitbox : MonoBehaviour
{
    public Syringe parent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(parent.damage, transform.position);
        }
    }
}
