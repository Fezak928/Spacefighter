using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseMovementController : MonoBehaviour
{
    protected Rigidbody2D EntityRigidbody;

    protected virtual void Awake()
    {
        EntityRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Move(Vector2 velocity)
    {
        Vector2 startingPosition = EntityRigidbody.position;

        startingPosition = ClampToPixelPerfectUnits(startingPosition, GameManager.instance.PixelsPerUnit);
        velocity = ClampToPixelPerfectUnits(velocity, GameManager.instance.PixelsPerUnit);

        EntityRigidbody.MovePosition(startingPosition + velocity);
    }

    protected Vector2 ClampToPixelPerfectUnits(Vector2 vector, int pixelsPerUnit)
    {
        return PixelPerfectClamp.ClampVector2ToPixelUnit(vector, pixelsPerUnit);
    }
}
