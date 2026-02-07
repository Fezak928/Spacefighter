using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid Data", menuName = "Asteroid Data")]
public class DamageableEntityDataSO : ScriptableObject
{
    [Min(1)] public int HitPoints = 3;

    [Min(0.01f)] public float MovementSpeed = 10f;
}
