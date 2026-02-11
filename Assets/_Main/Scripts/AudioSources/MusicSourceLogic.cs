using System.Collections;
using UnityEngine;

public class MusicSourceLogic : AudioSourceLogic
{
    public override IEnumerator ReturnToPoolAfterPlayingClip(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        MusicManager.Instance.PlayLoopingTrack();
        yield return null;
        ObjectPoolingManager.ReturnObjectToPool(gameObject, ObjectPoolingManager.PoolType.SFXs);
        
    }
}
