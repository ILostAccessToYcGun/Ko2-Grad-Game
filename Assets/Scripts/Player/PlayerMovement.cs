using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovement : MonoBehaviour
{

    public InputAction movement;
    public InputAction pause;
    [Space]
    [Header("Components")]
    PlayerStats player;
    public bool canMove;
    public FlipAnimation flip;
    public Sprite currentFrame1;
    public Sprite currentFrame2;
    [SerializeField] Sprite baseFrame1;
    [SerializeField] Sprite baseFrame2;
    public float baseFrameDelay;

    public TrailRenderer AOTtrail;
    public TrailRenderer Anesthetictrail;
    public TrailRenderer Fliptrail;

    [Header("Debug")]
    public Vector2 moveDir;

    public GameObject weaponToEquip;
    public float timer = 0.0f;
    bool pauseDown = false;

    private void Start()
    {
        player = GameManager.instance.playerStats;
    }

    private void OnEnable()
    {
        movement.Enable();
        pause.Enable();
        //mousePos.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        pause.Disable();
        //mousePos.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.ReadValue<float>() > 0.0f && !pauseDown)
        {
            pauseDown = true;
            if (GameManager.instance.gameState == GameManager.States.Paused)
            {
                GameManager.instance.Unpause();
            }
            else
            {
                GameManager.instance.Pause();
            }
                
        }

        if (pause.ReadValue<float>() == 0.0f)
        {
            pauseDown = false;
        }

        if (!canMove) return;
        moveDir = movement.ReadValue<Vector2>().normalized * Time.deltaTime * player.SPD * player.speedMult;
        transform.position += (Vector3)moveDir;

        if (moveDir != Vector2.zero)
        {
            if (timer >= baseFrameDelay * (GameManager.instance.playerStats.SPD * 0.25f))
            {
                timer = 0.0f;
                if (GameManager.instance.playerStats.sprite.sprite == currentFrame1) GameManager.instance.playerStats.sprite.sprite = currentFrame2;
                else GameManager.instance.playerStats.sprite.sprite = currentFrame1;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void ResetPosition()
    {
        moveDir = Vector3.zero;
        transform.position = new Vector3(-10.0f, -4.8f, 0.0f);
    }

    public void ResetFrames()
    {
        currentFrame1 = baseFrame1;
        currentFrame2 = baseFrame2;
    }
}
