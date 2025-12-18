using UnityEngine;

public class SpinProjectile : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;
    public int rightMult;

    public AudioSource spin;

    private void Start()
    {
        spin.pitch = Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, rightMult * GameManager.instance.playerStats.SPD * Time.deltaTime * 300.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, transform.position, 2);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - GameManager.instance.playerMovement.transform.position) * knockback;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
