using System.Collections;
using UnityEngine;

public class BassSmash : MonoBehaviour
{
    public Animation swingAnimation;
    public AnimationClip left;
    public AnimationClip right;
    public GameObject bassShockwave;
    public SpriteRenderer sprite;
    public Vector2 moveDir = new Vector2(1, 1);
    public float damage;
    public float knockback;
    public float stunTime;
    public float offset;
    public float startDistance;

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }

    public void HideSelf()
    {
        sprite.enabled = false;
    }

    public void StartShockwave()
    {
        StartCoroutine(ShockWave());
    }

    IEnumerator ShockWave()
    {
        float timer = 0.1f;

        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
        {
            //spawn a shockwave in the moveDir direction and increase the offsett by 
            BassSmashDamage dmg = Instantiate(bassShockwave, (Vector2)GameManager.instance.weaponHand.transform.position + (moveDir * startDistance) + (moveDir * offset * i), Quaternion.identity).GetComponent<BassSmashDamage>();
            dmg.damage = damage;
            dmg.stunTime = stunTime;
            dmg.knockback = knockback;
            dmg.moveDir = moveDir;

            timer = 0.1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        yield return null;
        DestroySelf();
    }
}
