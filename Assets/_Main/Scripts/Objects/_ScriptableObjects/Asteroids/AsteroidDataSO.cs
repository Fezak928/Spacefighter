using UnityEngine;

[CreateAssetMenu(fileName = "New Asteroid Data", menuName = "Object Datas/Asteroid Data")]
public class AsteroidDataSO : ProjectileDataSO
{
    [Min(1)] public int HitPoints = 3;

    [Min(10)] public int ScoreValue = 100;
}
