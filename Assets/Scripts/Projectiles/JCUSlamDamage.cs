using UnityEngine;

public class JCUSlamDamage : MonoBehaviour
{
    public float damage;
    public float knockback;
    public GameObject particle;
    public Vector3 spawnOffset;

    private void Start()
    {
        Invoke("DestroySelf", 0.5f);
        GameObject part = Instantiate(particle, transform.position + spawnOffset, Quaternion.identity);
        CameraShake.instance.Shake(0.2f, 0.4f);

        AudioManager.instance.Play("Slam", 0.75f, 1.25f);
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
