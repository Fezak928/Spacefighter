using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Object Datas/Projectile Data")]
public class ProjectileDataSO : CoreObjectDataSO
{
    [Min(1)] public int Damage = 1;
    [Range(0f, 1f)] public float ShakeMagnitude = 0.1f;
    [Range(0f, 1f)] public float HitStopDuration;
}
