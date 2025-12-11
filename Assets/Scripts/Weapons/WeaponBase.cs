using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{
    public InputAction m1;
    public InputAction m2;
    public PlayerStats playerStats;
    public PlayerMovement playerMovement;

    public float attackTimer;
    public float m1CD;
    public float m2CD;

    private void Start()
    {
        playerStats = GameManager.instance.playerStats;
        playerMovement = GameManager.instance.playerMovement;
    }

    private void OnEnable()
    {
        m1.Enable();
        m2.Enable();
    }

    private void OnDisable()
    {
        m1.Disable();
        m2.Disable();
    }

    protected virtual void OnUpdate()
    {
        // Default behavior (optional)
        Debug.Log("m1: " + m1.ReadValue<float>() + " - m2: " + m2.ReadValue<float>());
    }

    private void Update() 
    {
        OnUpdate();
    }
}
