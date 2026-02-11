using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioClip _startingTrack, _loopingTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        Instance = this;
    }

    private void Start()
    {
        PlayStartingTrack();
    }

    public void PlayStartingTrack()
    {
        AudioManagerSO.PlayMusic(_startingTrack, transform.position, 1f);
    }

    public void PlayLoopingTrack()
    {
        AudioManagerSO.PlayMusic(_loopingTrack, transform.position, 1f, true);
    }
}
