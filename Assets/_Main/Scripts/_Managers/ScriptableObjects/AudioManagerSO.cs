using UnityEngine;

[CreateAssetMenu(fileName = "_AudioManager", menuName = "_AudioManager")]
public class AudioManagerSO : ScriptableObject
{
    public AudioSource SoundObject;

    private static AudioManagerSO _instance;
    public static AudioManagerSO Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<AudioManagerSO>("_AudioManager");

            return _instance;
        }
    }

    private static float _volumeChange = 0.15f;
    private static float _pitchChange = 0.15f;

    public static void PlaySFX(AudioClip clip, Vector3 position, float volume)
    {
        float randomVolume = Random.Range(volume - _volumeChange, volume + _volumeChange);
        float randomPitch = Random.Range(1 - _pitchChange, 1 + _pitchChange);

        AudioSource source = ObjectPoolingManager.SpawnObject(Instance.SoundObject, position, Quaternion.identity, ObjectPoolingManager.PoolType.SFXs);

        AudioSourceLogic sfxLogic = source.gameObject.GetComponent<AudioSourceLogic>();

        sfxLogic.StartCoroutine(sfxLogic.ReturnToPoolAfterPlayingClip(clip.length));

        source.clip = clip;
        source.volume = randomVolume;
        source.pitch = randomPitch;
        source.Play();
    }

    public static void PlaySFX(AudioClip[] clips, Vector3 position, float volume)
    {
        int clipID = Random.Range(0, clips.Length);
        float randomVolume = Random.Range(volume - _volumeChange, volume + _volumeChange);
        float randomPitch = Random.Range(1 - _pitchChange, 1 + _pitchChange);

        AudioSource source = ObjectPoolingManager.SpawnObject(Instance.SoundObject, position, Quaternion.identity, ObjectPoolingManager.PoolType.SFXs);

        AudioSourceLogic sfxLogic = source.gameObject.GetComponent<AudioSourceLogic>();

        sfxLogic.StartCoroutine(sfxLogic.ReturnToPoolAfterPlayingClip(clips[clipID].length));

        source.clip = clips[clipID];
        source.volume = randomVolume;
        source.pitch = randomPitch;
        source.Play();
    }
}
