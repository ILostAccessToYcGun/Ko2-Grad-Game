using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovement : MonoBehaviour
{

    public InputAction movement;
    [Space]
    [Header("Components")]
    PlayerStats player;
    public bool canMove;
    [Header("Debug")]
    public Vector2 moveDir;

    public Sprite frame1;
    public Sprite frame2;
    public float baseFrameDelay;
    public float frameDelay;

    public GameObject test;

    public float timer = 0.0f;

    private void Start()
    {
        player = GameManager.instance.playerStats;
        GameManager.instance.playerStats.SetCurrentWeapon(test);

        frameDelay = baseFrameDelay * (GameManager.instance.playerStats.SPD * 0.25f);
    }

    private void OnEnable()
    {
        movement.Enable();
        //mousePos.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        //mousePos.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        moveDir = movement.ReadValue<Vector2>().normalized * Time.deltaTime * player.SPD;
        transform.position += (Vector3)moveDir;

        if (moveDir != Vector2.zero)
        {
            if (timer >= frameDelay)
            {
                timer = 0.0f;
                if (GameManager.instance.playerStats.sprite.sprite == frame1) GameManager.instance.playerStats.sprite.sprite = frame2;
                else GameManager.instance.playerStats.sprite.sprite = frame1;
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
        transform.position = Vector3.zero;
    }

}
