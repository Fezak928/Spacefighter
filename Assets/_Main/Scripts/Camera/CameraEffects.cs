using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance;
    private bool _performingHitstop, _shaking;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        Instance = this;
    }

    public void PerformHitstop(float duration)
    {
        if (!_performingHitstop)
        {
            Time.timeScale = 0.0f;
            StartCoroutine(HoldHitstop(duration));
        }
    }

    IEnumerator HoldHitstop(float duration)
    {
        _performingHitstop = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        _performingHitstop = false;
    }

    public void PerformCameraShake(float duration, float magnitude)
    {
        if (!_shaking)
            StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
