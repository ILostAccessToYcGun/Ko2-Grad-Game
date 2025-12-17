using UnityEngine;

public class BookSlamDamage : MonoBehaviour
{
    public float damage;
    public float knockback;
    public GameObject particle;
    public Vector3 spawnOffset;

    private void Start()
    {
        Invoke("DestroySelf", 0.5f);
        CameraShake.instance.Shake(0.025f, 0.05f);
        GameObject part = Instantiate(particle, transform.position + spawnOffset, Quaternion.identity);
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
