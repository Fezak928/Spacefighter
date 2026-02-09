using UnityEngine;

[CreateAssetMenu(fileName = "New Asteroid Data", menuName = "Object Datas/Asteroid Data")]
public class AsteroidDataSO : ProjectileDataSO
{
    [Range(0f, 5f)] public float MovementSpeedChange = 0.15f;

    [Min(1)] public int HitPoints = 3;
    [Min(10)] public int ScoreValue = 100;
}
