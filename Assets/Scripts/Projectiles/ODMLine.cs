using Unity.VisualScripting;
using UnityEngine;
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

    private void Start()
    {
       rb.angularVelocity = Random.Range(-200.0f, 200.0f);
       rb.linearVelocity = Random.insideUnitCircle * 5.0f;
       Invoke("Dash", Random.Range(0.2f, 0.4f));
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
            Debug.Log("ATTACH");
            attached = true;
            dashing = false;
            parent.ODMsConnected++;
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
