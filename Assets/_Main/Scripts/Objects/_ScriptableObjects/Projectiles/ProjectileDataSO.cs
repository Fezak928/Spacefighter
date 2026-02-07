using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Object Datas/Projectile Data")]
public class ProjectileDataSO : CoreObjectDataSO
{
    [Min(1)] public int Damage = 1;
}
