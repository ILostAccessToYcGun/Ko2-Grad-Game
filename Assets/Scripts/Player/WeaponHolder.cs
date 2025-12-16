using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject holder;
    public GameObject swordHolder;
    [SerializeField] float holdDistance;
    void Update()
    {
        if (GameManager.instance.gameState != GameManager.States.Playing) return;
        holder.transform.position = GameManager.instance.playerMovement.transform.position + ((Vector3)GameManager.instance.cam.screenToWorld - GameManager.instance.playerMovement.transform.position).normalized * holdDistance;
    }
}
