using UnityEngine;

[CreateAssetMenu(fileName = "_PlayerData", menuName = "_PlayerData")]
public class PlayerDataSO : CoreObjectDataSO
{
    [Range(0f, 1f)] public float InputThreshold = 0.25f;

    [Range(1,5)] public int HitPoints = 5;
    [Min(0.01f)] public float FireRate = 0.125f;
    [Range(1f, 5f)] public float InvincibilityDuration = 3f;

    public AudioClip[] DeadSFXClips;
}
