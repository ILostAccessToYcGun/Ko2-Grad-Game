using UnityEngine;

public class HelperManager : MonoBehaviour
{
    public static HelperManager instance;
    private void Awake()
    {
        instance = this;
    }

    public Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad; // Convert degrees to radians
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // Apply rotation matrix:
        // newX = x * cos - y * sin
        // newY = x * sin + y * cos
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    public Transform RotateTowardsDirection(Vector2 direction, Transform transform)
    {
        if (direction != Vector2.zero)
        {
            Transform returnTrans = transform;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation around the Z-axis
            returnTrans.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
            return returnTrans;
        }
        return null;
    }
}
