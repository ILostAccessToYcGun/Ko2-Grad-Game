using UnityEngine;

public class EXPHit : MonoBehaviour
{
    [SerializeField] EXPCrystal main;

    private void Start()
    {
        main = GetComponentInParent<EXPCrystal>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.GainEXP(main.EXP);
            Destroy(main.gameObject);
        }
    }
}
