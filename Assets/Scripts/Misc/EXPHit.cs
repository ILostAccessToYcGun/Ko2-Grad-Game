using UnityEngine;

public class EXPHit : MonoBehaviour
{
    [SerializeField] EXPCrystal main;

    bool isBeingPickedUp = false;

    private void Start()
    {
        main = GetComponentInParent<EXPCrystal>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null && !isBeingPickedUp)
        {
            isBeingPickedUp = true;
            player.GainEXP(main.EXP);
            Invoke("DestroySelf", 0.5f);

            AudioManager.instance.Play("EXP", 0.75f, 1.25f);
        }
    }

    void DestroySelf()
    {
        Destroy(main.gameObject);
    }
}
