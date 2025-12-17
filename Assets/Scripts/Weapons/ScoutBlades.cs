using System.Collections;
using System.Xml;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ScoutBlades : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] SpriteRenderer sprite1;
    [SerializeField] SpriteRenderer sprite2;
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float m1Knockback;
    int swingAnimation = 0;
    public bool hitEnemy = false;
    public int bladeCounter = 0;
    public int bladeAmmo = 5;
    bool ejectFrag = false;
    [SerializeField] GameObject fragment;



    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.0f;
    [SerializeField] float accuracyRange = 5.0f;
    [SerializeField] float dashSpeed = 80.0f;
    [SerializeField] float kickDistance = 10.0f;
    bool ODMright;
    bool ODMing = false;
    public int ODMsConnected = 0;
    public GameObject ODMtarget;
    public GameObject ODM1;
    public GameObject ODM2;
    [SerializeField] float m2Knockback;
    bool dashSlash = false;
    //[SerializeField] float speed;
    //[SerializeField] float acceleration;
    //[SerializeField] float damageInterval;
    //[SerializeField] float duration;
    //[SerializeField] float angleSpread = 45.0f;
    //[SerializeField] float rotationSpeed = 300f;

    private void Start()
    {
        base.Start();
        UIManager.instance.ToggleBladeUI();
        UIManager.instance.UpdateBladeCount(bladeAmmo);

    }

    protected override void OnUpdate()
    {
        Vector2 forwardVector = (GameManager.instance.cam.screenToWorld - (Vector2)playerMovement.transform.position).normalized;
        if (GameManager.instance.gameState == GameManager.States.Playing)
            HelperManager.instance.RotateTowardsDirection(forwardVector, GameManager.instance.weaponHand.transform);

        if (ODMing)
        {
            if (ODMsConnected == 2)
            {
                playerMovement.canMove = false;
                playerStats.canTakeDamage = false;

                playerMovement.transform.position = Vector2.MoveTowards(playerMovement.transform.position, ODMtarget.transform.position, dashSpeed * Time.deltaTime);

                //DASH SLASH
                if (Vector2.Distance(playerMovement.transform.position, ODMtarget.transform.position) < 2.5f && dashSlash == false)
                {
                    for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
                    {
                        ScoutSwing swing =
                            Instantiate(m1Projectile, transform.position, Quaternion.identity, transform)
                            .GetComponentInChildren<ScoutSwing>();

                        HelperManager.instance.RotateTowardsDirection(((Vector2)ODMtarget.transform.position - (Vector2)GameManager.instance.playerMovement.transform.position).normalized, swing.transform.parent);
                        swing.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                        swing.knockback = m2Knockback;
                        swing.parent = this;

                        swing.swingAnimation.clip = swing.second;

                        swing.swingAnimation.Play();
                        dashSlash = true;
                    }
                }

                //BOUNCE
                if (Vector2.Distance(playerMovement.transform.position, ODMtarget.transform.position) < 0.5f)
                {
                    playerMovement.GetComponent<Rigidbody2D>().linearVelocity = -((Vector2)ODMtarget.transform.position - (Vector2)GameManager.instance.playerMovement.transform.position).normalized * kickDistance;
                    Destroy(ODM1.gameObject);
                    Destroy(ODM2.gameObject);
                    ODMsConnected = 0;
                    ODMing = false;
                    playerMovement.canMove = true;
                    Invoke("DelayedEnableDamage", 0.5f);

                    bladeAmmo--;
                    UIManager.instance.UpdateBladeCount(bladeAmmo);
                    GameObject frag = Instantiate(fragment, playerMovement.transform.position, Quaternion.identity);
                    frag.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-200.0f, 200.0f);
                    frag.GetComponent<Rigidbody2D>().linearVelocity = Random.insideUnitCircle * 10.0f;
                    frag.GetComponent<BladeFragment>().parent = this;
                }
                Debug.Log("FLYYYYYYYYYYYYYYY");
                //FLY AT THEM
            }
        }

        if (hitEnemy == true && swingAnimation == 3 && ejectFrag == false)
        {
            bladeAmmo--;
            UIManager.instance.UpdateBladeCount(bladeAmmo);
            GameObject frag = Instantiate(fragment, playerMovement.transform.position, Quaternion.identity);
            frag.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-200.0f, 200.0f);
            frag.GetComponent<Rigidbody2D>().linearVelocity = Random.insideUnitCircle * 10.0f;
            frag.GetComponent<BladeFragment>().parent = this;
            ejectFrag = true;
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime * GameManager.instance.playerStats.ATKSPD;
        }
        else
        {
            if (GameManager.instance.gameState != GameManager.States.Playing) return;
            
            if (m1.ReadValue<float>() > 0.0f)
            {
                ejectFrag = false;
                if (bladeAmmo <= 0)
                {
                    Debug.Log("NO AMMO");
                    return;
                }
                //M1 attack
                StartCoroutine(SwingAttack(forwardVector));
                attackTimer = m1CD;
            }

            else if (m2.ReadValue<float>() > 0.0f)
            {
                if (bladeAmmo <= 0)
                {
                    Debug.Log("NO AMMO");
                    return;
                }

                float distance = 0.0f;
                ODMtarget = null;
                dashSlash = false;
                foreach (GameObject enemy in EnemyManager.instance.Enemies)
                {
                    distance = Vector2.Distance(GameManager.instance.cam.screenToWorld, enemy.transform.position);
                    if (distance < accuracyRange)
                    {

                        //outside of range check
                        if (Mathf.Abs(enemy.transform.position.y) > 26.2f ||
                            Mathf.Abs(enemy.transform.position.x) > 39.0f) continue;

                        ODMtarget = enemy;
                        break;
                    }
                }
                if (ODMtarget == null)
                {
                    Debug.Log("not an enemy, try aiming better");
                }
                else
                {
                    ODMing = true;
                    ODMright = (Random.Range(1, 3) == 1 ? true : false);
                    Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
                    if (ODMright) offset.x = 0.5f;
                    else offset.x = -0.5f;

                    ODMLine odm = Instantiate(m2Projectile, playerMovement.transform.position + offset, Quaternion.identity, playerMovement.transform).GetComponent<ODMLine>();
                    ODM1 = odm.gameObject;
                    odm.parent = this;
                    odm.target = ODMtarget;
                    odm.offset = offset;

                    Invoke("ODMDelay", Random.Range(0.2f, 0.4f));
                }
                attackTimer = m2CD;
            }
        }
    }

    IEnumerator SwingAttack(Vector2 forward)
    {
        sprite1.enabled = false;
        sprite2.enabled = false;
        
        float timer = 0.1f;
        hitEnemy = false;
        swingAnimation++;
        if (swingAnimation > 3)
        {
            swingAnimation = 1;
        }

        for (int i = 0; i < GameManager.instance.playerStats.PRJ; i++)
        {
            ScoutSwing swing =
                    Instantiate(m1Projectile, transform.position, Quaternion.identity, transform)
                    .GetComponentInChildren<ScoutSwing>();

            HelperManager.instance.RotateTowardsDirection((GameManager.instance.cam.screenToWorld - (Vector2)GameManager.instance.playerMovement.transform.position).normalized, swing.transform.parent);
            swing.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
            swing.knockback = m1Knockback;
            swing.parent = this;
            bladeCounter++;

            //Debug.Log(swingAnimation);
            if (swingAnimation == 1) swing.swingAnimation.clip = swing.first;
            else if (swingAnimation == 2) swing.swingAnimation.clip = swing.second;
            else swing.swingAnimation.clip = swing.third;

            swing.swingAnimation.Play();

            timer = 0.1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
       
        sprite1.enabled = true;
        sprite2.enabled = true;

        yield return null;
    }

    void ODMDelay()
    {
        Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
        if (!ODMright) offset.x = 0.5f;
        else offset.x = -0.5f;
        ODMLine odm = Instantiate(m2Projectile, playerMovement.transform.position + offset, Quaternion.identity, playerMovement.transform).GetComponent<ODMLine>();
        ODM2 = odm.gameObject;
        odm.parent = this;
        odm.target = ODMtarget;
        odm.offset = offset;

    }

    void DelayedEnableDamage()
    {
        playerStats.canTakeDamage = true;
    }
}
