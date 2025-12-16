using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleProjectile : MonoBehaviour
{
    public float damage;
    public float rotationSpeed = 2.0f;
    public float baseInterval = 3f;
    public float knockback = 0.5f;
    public float DOTInterval;

    private void Start()
    {
        StartCoroutine(ReactivateHitBox());
    }

    public List<GameObject> hitTargets;
    void Update()
    {
        if (rotationSpeed == 0.0f)
            DOTInterval = baseInterval / Mathf.Clamp(Mathf.Abs(0.00001f * 0.5f), 1.0f, 100.0f);
        else
            DOTInterval = baseInterval / Mathf.Clamp(Mathf.Abs(rotationSpeed * 0.5f), 1.0f, 100.0f);

        DOTInterval = Mathf.Clamp(DOTInterval, 0.0f, 5.0f);

        transform.Rotate(new Vector3(0.0f, 0.0f, -rotationSpeed));
        GameManager.instance.playerStats.sprite.transform.Rotate(new Vector3(0.0f, 0.0f, -rotationSpeed));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Mathf.Abs(rotationSpeed) < 0.5f) return;  
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            if (!hitTargets.Contains(collision.gameObject))
            {
                hitTargets.Add(collision.gameObject);
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.gameObject.transform.position - transform.position).normalized * knockback;
                dmg.TakeDamage(damage);
            }
        }
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

    private void OnDestroy()
    {
        GameManager.instance.playerStats.sprite.transform.rotation = Quaternion.identity;
    }
}
