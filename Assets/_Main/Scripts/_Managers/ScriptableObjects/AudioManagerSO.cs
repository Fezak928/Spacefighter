using UnityEngine;

[CreateAssetMenu(fileName = "_AudioManager", menuName = "_AudioManager")]
public class AudioManagerSO : ScriptableObject
{
    public AudioSource SoundObject;
    public AudioSource MusicObject;
    [Range(0f, 1f)] public float SFXVolume = 0.5f;

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

    private static float _volumeChange = 0.0375f;
    private static float _pitchChange = 0.1f;

    public static void PlaySFX(AudioClip clip, Vector3 position, float volume)
    {
        float randomVolume = Random.Range(Instance.SFXVolume * volume - _volumeChange, Instance.SFXVolume * volume + _volumeChange);
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
        float randomVolume = Random.Range(Instance.SFXVolume * volume - _volumeChange, Instance.SFXVolume * volume + _volumeChange);
        float randomPitch = Random.Range(1 - _pitchChange, 1 + _pitchChange);

        AudioSource source = ObjectPoolingManager.SpawnObject(Instance.SoundObject, position, Quaternion.identity, ObjectPoolingManager.PoolType.SFXs);

        AudioSourceLogic sfxLogic = source.gameObject.GetComponent<AudioSourceLogic>();

        sfxLogic.StartCoroutine(sfxLogic.ReturnToPoolAfterPlayingClip(clips[clipID].length));

        source.clip = clips[clipID];
        source.volume = randomVolume;
        source.pitch = randomPitch;
        source.Play();
    }

    public static void PlayMusic(AudioClip clip, Vector3 position, float volume, bool loopeable = false)
    {
        AudioSource source = ObjectPoolingManager.SpawnObject(Instance.MusicObject, position, Quaternion.identity, ObjectPoolingManager.PoolType.SFXs);

        MusicSourceLogic musicLogic = source.gameObject.GetComponent<MusicSourceLogic>();

        if (!loopeable)
            musicLogic.StartCoroutine(musicLogic.ReturnToPoolAfterPlayingClip(clip.length));

        source.loop = loopeable;
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }
}
