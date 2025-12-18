using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private void Awake()
    {
        instance = this;
    }

    public void Shake(float magnitude, float duration)
    {
        StartCoroutine(ShakeCamera(magnitude, duration));
    }

    IEnumerator ShakeCamera(float magnitude, float duration)
    {
        float timer = duration;
        while ( timer >= 0)
        {
            if (GameManager.instance.gameState != GameManager.States.Playing) break;
            timer -= Time.deltaTime;
            transform.localPosition = Random.insideUnitCircle * magnitude;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        yield return null;
    }
}
