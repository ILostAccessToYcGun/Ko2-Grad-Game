using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public InputAction movement;
    [Space]
    [Header("Components")]
    PlayerStats player;
    [Header("Debug")]
    public Vector2 moveDir;
    //public InputAction mousePos;

    private void Start()
    {
        player = GameManager.instance.playerStats;
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
        moveDir = movement.ReadValue<Vector2>().normalized * Time.deltaTime * player.SPD;
        transform.position += (Vector3)moveDir;
    }
}
