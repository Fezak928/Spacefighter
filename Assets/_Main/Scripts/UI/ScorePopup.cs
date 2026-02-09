using UnityEngine;
using System.Collections;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 0.5f;
    [SerializeField] private TextMeshProUGUI _text;

    public IEnumerator ReturnToPool(int score)
    {
        _text.text = $"+{score}";

        yield return new WaitForSecondsRealtime(_lifeTime);

        ObjectPoolingManager.ReturnObjectToPool(gameObject, ObjectPoolingManager.PoolType.VFXs);
    }
}
