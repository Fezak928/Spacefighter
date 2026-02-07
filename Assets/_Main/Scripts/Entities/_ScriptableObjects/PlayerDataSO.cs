using UnityEngine;

[CreateAssetMenu(fileName = "_PlayerData", menuName = "_PlayerData")]
public class PlayerDataSO : DamageableEntityDataSO
{

    [Range(0f, 1f)] public float InputThreshold = 0.25f;

    [Min(0.01f)] public float FireRate = 0.125f;
}
