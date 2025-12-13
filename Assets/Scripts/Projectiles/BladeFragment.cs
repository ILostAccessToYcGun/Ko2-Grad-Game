using UnityEngine;

public class BladeFragment : MonoBehaviour
{
    public CircleCollider2D col;
    public ScoutBlades parent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("EnableCollider", 0.5f);
    }

    void EnableCollider()
    {
        col.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            parent.bladeAmmo++;
            Destroy(gameObject);
        }
    }
}
