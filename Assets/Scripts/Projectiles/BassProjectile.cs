using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassProjectile : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float speed = 2.0f;
    public float knockback = 2.0f;
    public float duration = 60.0f;
    public float DOTInterval = 0.5f;
    public float sizeGain = 0.1f;

    private void Start()
    {
        HelperManager.instance.RotateTowardsDirection(moveDir, transform);
        Invoke("DestroySelf", duration);
        StartCoroutine(ReactivateHitBox());
    }

    public List<GameObject> hitTargets;
    void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;
        transform.localScale += new Vector3(1, 1, 1) * sizeGain * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            if (!hitTargets.Contains(collision.gameObject))
            {
                hitTargets.Add(collision.gameObject);
                dmg.TakeDamage(damage, transform.position);
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moveDir * knockback;
            }

        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    IEnumerator ReactivateHitBox()
    {
        float timer = 0.0f;
        while(true)
        {
            if (timer < DOTInterval) timer += Time.deltaTime;
            else
            {
                
                hitTargets.Clear();
                timer = 0.0f;
            }
            yield return null;
        }
    }
}
