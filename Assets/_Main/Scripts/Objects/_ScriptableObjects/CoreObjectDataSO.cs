using UnityEngine;

public abstract class CoreObjectDataSO : ScriptableObject
{
    [Min(0.01f)] public float MovementSpeed = 10f;

    public AudioClip[] clips;
}
