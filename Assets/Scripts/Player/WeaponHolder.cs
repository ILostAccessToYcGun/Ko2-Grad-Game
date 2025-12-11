using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject holder;
    [SerializeField] float holdDistance;
    void Update()
    {
        holder.transform.position = GameManager.instance.playerMovement.transform.position + ((Vector3)GameManager.instance.cam.screenToWorld - GameManager.instance.playerMovement.transform.position).normalized * holdDistance;
    }
}
