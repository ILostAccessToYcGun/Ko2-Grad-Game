using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements.Experimental;

public class ODMLine : MonoBehaviour
{
    public ScoutBlades parent;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Rigidbody2D rb;
    public GameObject target;
    public float speed = 25f;
    bool dashing = false;
    bool attached = false;
    public Vector3 offset;

    public GameObject sparkingPart;
    public GameObject currentSpark;
    public GameObject gougePart;

    private void Start()
    {
        rb.angularVelocity = Random.Range(-200.0f, 200.0f);
        rb.linearVelocity = Random.insideUnitCircle * 10.0f;
        Invoke("Dash", Random.Range(0.2f, 0.4f));
        currentSpark = Instantiate(sparkingPart, GameManager.instance.playerMovement.transform.position + offset, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, GameManager.instance.playerMovement.transform.position + offset);
        if (attached && target != null)
        {
            transform.position = target.transform.position;
        }

        lineRenderer.SetPosition(1, transform.position);


        if (Vector2.Distance(transform.position, target.transform.position) < 0.5f && !attached)
        {
            Destroy(currentSpark);
            Debug.Log("ATTACH");
            attached = true;
            dashing = false;
            parent.ODMsConnected++;

            AudioManager.instance.Play("ODMHit", 0.9f, 1.1f);

            GameObject ouch = Instantiate(gougePart, target.transform.position, Quaternion.identity);
            ouch.transform.localScale = target.GetComponentInChildren<SpriteRenderer>().transform.localScale;
            HelperManager.instance.RotateTowardsDirection((target.transform.position - transform.position).normalized, ouch.transform);
        }

        if (dashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    void Dash()
    {
        Debug.Log("DASH");
        dashing = true;
    }
}
