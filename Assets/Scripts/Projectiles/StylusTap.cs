using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class StylusTap : MonoBehaviour
{
    public CircleCollider2D col;
    public SpriteRenderer sprite;
    public RhythmStylus parent;
    //public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;

    public GameObject target;
    public float smallestDist = 10.0f;
    bool isDisabling = false;

    private void Start()
    {
        Invoke("DestroySelf", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //basically the trigger will be on the moment we spawn in, and when we hit target(s) find the closest one,
        //damage it and place it into the last hit from the parent.
        //not sure how the multi projectiels are gona work, maybe it onyl gets added if its the last one?

        //when this spawns in , scan the collided area for all targets
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            Debug.Log("hit");
            if (!isDisabling) StartCoroutine(TurnOffNextFrame());
            //turn the collider off next frame
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            if (distance < smallestDist)
            {
                smallestDist = distance;
                target = collision.gameObject;
            }
        }
    }

    void DestroySelf()
    {
        StartCoroutine(TurnOffNextFrame());
    }

    IEnumerator TurnOffNextFrame()
    {
        isDisabling = true;
        yield return new WaitForFixedUpdate();
        col.enabled = false;
        yield return new WaitForFixedUpdate();

        if (target != null)
        {
            float mult = 1.0f;
            for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
            {
                if (target == parent.lastHit)
                {
                    mult = 0.1f;
                }
                target.GetComponent<IDamage>().TakeDamage(damage * mult, transform.position);
                target.GetComponent<Rigidbody2D>().linearVelocity = (target.transform.position - transform.position).normalized * knockback;
            }
            parent.lastHit = target;
        }
        Destroy(gameObject);
    }
}
