using UnityEngine;

public class Syringe : MonoBehaviour
{

    public float damage;
    public float beamDelay;
    public float totalTime;
    public float recoil = 3;
    public GameObject beam;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("MIKU_MIKU_BEEEEEEEEEEEEEEEEEM", beamDelay);
        Invoke("DestroySelf", totalTime);
    }

    void MIKU_MIKU_BEEEEEEEEEEEEEEEEEM()
    {
        beam.SetActive(true);
        rb.linearVelocity = transform.up * recoil;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }


}
