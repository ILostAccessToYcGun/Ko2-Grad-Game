using UnityEngine;

public class BladeFragment : MonoBehaviour
{
    public ScoutBlades parent;
    float timer = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer > 0) return;

        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            Debug.Log("++Ammo");
            parent.bladeAmmo++;
            UIManager.instance.UpdateBladeCount(parent.bladeAmmo);
            Destroy(gameObject);
        }
    }
}
