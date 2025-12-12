using UnityEngine;

public class SpinProjectile : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;
    public int rightMult;

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, rightMult * GameManager.instance.playerStats.SPD * Time.deltaTime * 300.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - GameManager.instance.playerMovement.transform.position) * knockback;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
