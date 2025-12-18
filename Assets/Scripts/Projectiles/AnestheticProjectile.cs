using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnestheticProjectile : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float speed = 2.0f;
    public float duration = 10.0f;
    public float DOTInterval = 0.5f;
    public float sizeGain = 0.1f;

    public GameObject part;
    GameObject currentPart;

    private void Start()
    {
        Invoke("DestroySelf", duration);
        StartCoroutine(ReactivateHitBox());
        currentPart = Instantiate(part, transform.position, Quaternion.identity);
    }

    public List<GameObject> hitTargets;
    void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;
        transform.localScale += new Vector3(1, 1, 1) * sizeGain * Time.deltaTime;
        currentPart.transform.position = transform.position;
        currentPart.transform.localScale = transform.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            if (!hitTargets.Contains(collision.gameObject))
            {
                hitTargets.Add(collision.gameObject);
                player.TakeHealing(damage * 0.02f);
            }
        }

        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            if (!hitTargets.Contains(collision.gameObject))
            {
                hitTargets.Add(collision.gameObject);
                dmg.TakeDamage(damage, transform.position);
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
