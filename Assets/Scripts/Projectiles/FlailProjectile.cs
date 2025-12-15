using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlailProjectile : MonoBehaviour
{
    [SerializeField] CyclingFlail parent;
    [SerializeField] LineRenderer line;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprite;
    public bool demonForm = false;
    public bool canMove = true;
    public float damage;
    public float DmgAmp;
    public float knockback;
    public float demonFollowDistance;
    public bool kicked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Move(Vector2 dir)
    {
        if (!canMove) return;
        rb.linearVelocity = dir + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        rb.angularVelocity = Random.Range(-200.0f, 200.0f);
        canMove = false;
        StartCoroutine(MoveCooldown());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            if (demonForm)
                dmg.TakeDamage(damage * DmgAmp);
            else
                dmg.TakeDamage(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - transform.position).normalized * knockback * (demonForm ? 2.0f : 1.0f);
        }
    }

    public void SetDemonForm(bool set, Color colour, Color chainColour)
    {
        demonForm = set;
        sprite.color = colour;
        line.startColor = colour;
        line.endColor = colour;
    }

    private void Update()
    {
        line.SetPosition(0, GameManager.instance.weaponHand.transform.position);
        line.SetPosition(1, transform.position);

        if (!demonForm) return;

        if (Vector2.Distance(transform.position, GameManager.instance.playerMovement.transform.position) >= demonFollowDistance)
        {
            if (!kicked)
                rb.AddForce((GameManager.instance.playerMovement.transform.position - transform.position) * GameManager.instance.playerStats.SPD * 0.5f);
            //transform.position = Vector2.MoveTowards();
        }

        
    }

    IEnumerator MoveCooldown()
    {
        yield return new WaitForSeconds(Random.Range(0.4f, 0.6f));
        canMove = true;
    }

    //public IEnumerator Kicked()
    //{
    //    kicked = true;
    //    yield return new WaitForSeconds(0.5f);
    //    Debug.Log("hehe");
    //    kicked = false;
    //}
    public void Kicked()
    {
        Invoke("KickFinish", 0.5f);
    }
    void KickFinish()
    {
        kicked = false;
    }
}
