using UnityEngine;
using System.Collections;

public class AudioSourceLogic : MonoBehaviour
{
    #region I don't know why this wasn't working
    //[SerializeField] private AudioSource _source;

    //private float _clipLength;

    //private void OnEnable()
    //{
    //    if (_source == null)
    //        _source = GetComponent<AudioSource>();

    //    _clipLength = _source.clip.length;
    //    StartCoroutine(ReturnToPoolAfterPlayingClip(_clipLength));
    //}
    #endregion

    public virtual IEnumerator ReturnToPoolAfterPlayingClip(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        ObjectPoolingManager.ReturnObjectToPool(gameObject, ObjectPoolingManager.PoolType.SFXs);
    }
}
