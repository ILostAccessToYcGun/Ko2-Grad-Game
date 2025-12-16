using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ArtisticSoles : WeaponBase
{
    public GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    public float m1DmgMult = 1.0f;
    [SerializeField] float flipCooldown = 0.28f;
    public int flipCounter = 0;
    public float consecutiveTimeFrame = 0.5f;
    public float consecutiveTimer = 0.0f;
    public bool checkConsecutive = false;

    [SerializeField] FlipAnimation flipper;
    [SerializeField] AnimationClip flipClock;
    [SerializeField] AnimationClip flipAnti;
    public bool canFlip = true;
    public Vector2 target;
    Vector2 current;
    public float m1Knockback;
    bool mouseDown;
     

    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 1.5f;
    [SerializeField] float m2Knockback = 1.5f;

    float currentRotationSpeed;
    [SerializeField] Vector2 rotationSpeedRange;
    [SerializeField] float rotationDrag = 1.0f;
    [SerializeField] float rotationStrength = 3.0f;
    [SerializeField] float speedReduction;
    CircleProjectile currentCircle;
    public bool isCircling = false;
    Vector2 lastDir;
    [Space]
    [SerializeField] Sprite gym1;
    [SerializeField] Sprite gym2;
    new public void Start()
    {
        base.Start();
        GameManager.instance.playerStats.sprite.sprite = gym1;
        GameManager.instance.playerMovement.currentFrame1 = gym1;
        GameManager.instance.playerMovement.currentFrame2 = gym2;
        flipper = GameManager.instance.playerMovement.flip;
        flipper.parent = this;
    }

    protected override void OnUpdate()
    {
        if (m1.ReadValue<float>() == 0.0f) mouseDown = false;

        if (!canFlip)
        {
            //Lerp from the current to the target
            playerMovement.transform.position = Vector2.Lerp(current, target, flipper.flipAnimation[flipper.flipAnimation.clip.name].time / flipper.flipAnimation[flipper.flipAnimation.clip.name].length);

        }

        if (checkConsecutive)
        {
            consecutiveTimer -= Time.deltaTime;
            //if consecutive did not fail, you dont go in here anymore and thus do not hit teh fail

            if (consecutiveTimer < 0.0f)
            {
                Debug.Log("Consecutive Failed");
                //go on long cooldown
                checkConsecutive = false;
                flipCounter = 0;
                attackTimer = m1CD;
            }
        }


        if (isCircling)
        {
            //Debug.Log(Vector2.SignedAngle(GameManager.instance.playerMovement.moveDir, lastDir));
            currentRotationSpeed *= rotationDrag;
            if (GameManager.instance.playerMovement.moveDir != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(GameManager.instance.playerMovement.moveDir, lastDir);

                if (Mathf.Abs(angle) > 0.1f && Mathf.Abs(angle) < 180.0f)
                {
                    if (angle < 0)
                    {
                        Debug.Log("angle--");
                        //anti clockwise, WASD
                        currentRotationSpeed -= rotationStrength * Time.deltaTime;
                    }
                    else if (angle > 0)
                    {
                        Debug.Log("angle++");
                        //clockwise, WDSA
                        currentRotationSpeed += rotationStrength * Time.deltaTime;
                    }
                }

                if (GameManager.instance.playerMovement.moveDir != Vector2.zero)
                    lastDir = GameManager.instance.playerMovement.moveDir;
            }
            currentCircle.rotationSpeed = currentRotationSpeed;
        }



        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime * playerStats.ATKSPD;
        }
        else
        {
            if (GameManager.instance.gameState != GameManager.States.Playing) return;
            Vector2 forwardVector = (GameManager.instance.cam.screenToWorld - (Vector2)playerMovement.transform.position).normalized;
            if (m1.ReadValue<float>() > 0.0f)
            {
                if (!canFlip) return;
                if (mouseDown) return;
                if (isCircling) return;
                isCircling = false;
                mouseDown = true;
                canFlip = false;
                playerMovement.canMove = false;
                playerStats.canTakeDamage = false;
                checkConsecutive = false;

                target = (Vector2)playerMovement.transform.position + (forwardVector * playerStats.SPD * 1.5f);
                current = playerMovement.transform.position;
                //M1 attack
                //consecutive flips
                

                //play the animation
                if (forwardVector.x > 0) flipper.flipAnimation.clip = flipClock;
                else flipper.flipAnimation.clip = flipAnti;

                flipper.flipAnimation.Play();
                flipCounter++;

                if (flipCounter < playerStats.PRJ)
                    attackTimer = flipCooldown;
                else
                {
                    attackTimer = m1CD;
                }
            }
            
            else if (m2.ReadValue<float>() > 0.0f)
            {
                //M2 attack
                //circles
                currentRotationSpeed = 0.1f;
                lastDir = GameManager.instance.playerMovement.moveDir;
                isCircling = !isCircling;
                

                if (isCircling)
                {
                    playerStats.speedMult = speedReduction;
                    if (currentCircle == null)
                    {
                        currentCircle = Instantiate(m2Projectile, playerMovement.transform.position, Quaternion.identity, playerMovement.transform).GetComponent<CircleProjectile>();
                        currentCircle.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                        currentCircle.knockback = m2Knockback;
                    }
                }
                else
                {
                    playerStats.speedMult = playerStats.baseSpeedMult;
                    Destroy(currentCircle.gameObject);
                }

                attackTimer = m2CD;
            }
        }
    }
}
