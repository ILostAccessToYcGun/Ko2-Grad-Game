using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public InputAction movement;
    [Space]
    [Header("Components")]
    PlayerStats player;
    public bool canMove;
    [Header("Debug")]
    public Vector2 moveDir;
    //public InputAction mousePos;

    public GameObject test;

    private void Start()
    {
        player = GameManager.instance.playerStats;
        GameManager.instance.playerStats.SetCurrentWeapon(test);
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
    }

    public void ResetPosition()
    {
        moveDir = Vector3.zero;
        transform.position = Vector3.zero;
    }
}
