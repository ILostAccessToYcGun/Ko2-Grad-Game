using UnityEngine;

public class JCUSlamDamage : MonoBehaviour
{
    public float damage;
    public float knockback;
    public GameObject particle;

    private void Start()
    {
        Invoke("DestroySelf", 0.5f);
        if (particle == null)
        {
            Debug.Log("no particle");
            return;
        }
        GameObject part = Instantiate(particle, transform.position + new Vector3(0.0f, -0.15f, 0.0f), Quaternion.identity);
        part.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
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
