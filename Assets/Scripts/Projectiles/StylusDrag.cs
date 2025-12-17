using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class StylusDrag : MonoBehaviour
{
    public RhythmStylus parent;
    public CircleCollider2D col;
    public SpriteRenderer sprite;
    public Color explodeColour;
    public Gradient lineExplodeColour;
    //public Vector2 moveDir = new Vector2(1, 1);
    public float damage;

    public bool canTrigger = false;
    public float triggerTime;

    public LineRenderer line;
    public GameObject front;
    //this is one detonation pad
    //projectile count will increase the number of times the explosion is triggered

    private void Update()
    {
        line.SetPosition(0, transform.position);

        if (front != null) line.SetPosition(1, front.transform.position);
        else line.SetPosition(1, transform.position);

    }

    public void TimeExplosion()
    {
        Invoke("Trigger", triggerTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, transform.position);
        }
    }

    void Trigger()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        Color orig = sprite.color;
        Gradient origLine = line.colorGradient;
        float timer = 0.0f;
        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
        {
            timer = 0.1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            col.enabled = true;
            sprite.color = explodeColour;
            line.colorGradient = lineExplodeColour;
            yield return new WaitForSeconds(0.05f);
            line.colorGradient = origLine;
            sprite.color = orig;
            col.enabled = false;

            
            yield return null;
        }
        parent.explosions.Remove(this);
        Destroy(gameObject);
    }
}
