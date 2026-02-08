using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseMovementController : MonoBehaviour
{
    protected Rigidbody2D ObjectRigidbody;

    protected virtual void Awake()
    {
        ObjectRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Move(Vector2 velocity)
    {
        Vector2 startingPosition = ObjectRigidbody.position;

        startingPosition = ClampToPixelPerfectUnits(startingPosition, GameManager.Instance.PixelsPerUnit);
        velocity = ClampToPixelPerfectUnits(velocity, GameManager.Instance.PixelsPerUnit);

        ObjectRigidbody.MovePosition(startingPosition + velocity);
    }

    protected Vector2 ClampToPixelPerfectUnits(Vector2 vector, int pixelsPerUnit)
    {
        return PixelPerfectClamp.ClampVector2ToPixelUnit(vector, pixelsPerUnit);
    }
}
