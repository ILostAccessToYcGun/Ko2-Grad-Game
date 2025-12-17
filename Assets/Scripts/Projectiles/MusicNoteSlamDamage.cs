using UnityEngine;

public class MusicNoteSlamDamage : MonoBehaviour
{
    MusicNoteSlam parent;
    private void Start()
    {
        parent = GetComponentInChildren<MusicNoteSlam>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(parent.damage, transform.position, 2);
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        }
    }
}
