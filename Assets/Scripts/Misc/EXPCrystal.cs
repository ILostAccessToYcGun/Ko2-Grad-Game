using UnityEngine;
using UnityEngine.tvOS;

public class EXPCrystal : MonoBehaviour
{
    [Header("Stats")]
    public float EXP = 5f;
    [SerializeField] float magnetStartSpeed = 0.25f;
    [SerializeField] float magnetAccel = 1.0f;
    [Header("Components")]
    public CircleCollider2D magnetCol;

    public bool hitPlayer = false;

    private void Start()
    {
        magnetCol = GetComponent<CircleCollider2D>();
        UpdateRange();
    }

    public void UpdateRange()
    {
        magnetCol.radius = GameManager.instance.playerStats.magnetRange;
    }

    private void Update()
    {
        if (hitPlayer)
        {
            //fly to the player
            magnetStartSpeed += Time.deltaTime * magnetAccel;
            transform.position = Vector2.MoveTowards(transform.position, GameManager.instance.playerMovement.transform.position, magnetStartSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) hitPlayer = true;
    }
}
