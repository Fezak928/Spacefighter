using UnityEngine;

public interface IMovable
{
    Vector2 Velocity { get; set; }

    void HandleVelocity(Vector2 direction);
}
