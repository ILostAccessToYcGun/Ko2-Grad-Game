using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTween : MonoBehaviour
{
    Camera cam;
    PlayerMovement player;
    public float followRatio = 0.5f;
    public float tweenSpeed = 0.1f;
    [Space]
    public Vector2 mousePos;
    public Vector2 screenToWorld;
    public InputAction mouse;
    float startOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        startOffset = cam.transform.position.z;
        player = GameManager.instance.playerMovement;
    }

    private void OnEnable()
    {
        mouse.Enable();
    }

    private void OnDisable()
    {
        mouse.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("huh");
        mouse.performed += MousePos;

        screenToWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 zRemove = Vector3.Lerp(player.transform.position, screenToWorld, followRatio);
        Vector3 currentPos = cam.transform.position;
        Vector3 targetPos = new Vector3(zRemove.x, zRemove.y, startOffset);

        cam.transform.position = currentPos + (targetPos - currentPos) * tweenSpeed * Time.deltaTime;

        //LeanTween.value(gameObject, currentPos, zRemove, tweenTime).setEaseInCubic().setOnUpdate((Vector3 val) => { cam.transform.position = val; });


        //Debug.Log(currentPos);
        //cam.transform.position = currentPos;

    }

    private void MousePos(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }
}
