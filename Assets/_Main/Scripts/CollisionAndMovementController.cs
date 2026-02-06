using System;
using UnityEngine;

public class CollisionAndMovementController : MonoBehaviour
{
    public const float CollisionPadding = 0.015f;

    [SerializeField]
    private LayerMask _boundsLayer;
    [Range(2, 100)] public int HorizontalRaysAmount = 4, VerticalRaysAmount = 4;

    private float _horizontalRaySpace, _verticalRaySpace;

    private BoxCollider2D _collider;
    
    public RaycastCorners RayCorners;
    public bool CollidingTop { get; private set; }
    public bool CollidingBottom { get; private set; }
    public bool CollidingLeft { get; private set; }
    public bool CollidingRight { get; private set; }

    private Rigidbody2D _rigidbody;

    public struct RaycastCorners
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CalculateRaySpacing();
    }

    public void Move(Vector2 velocity)
    {
        UpdateRaycastCorners();
        ResetCollisionStates();

        ResolveHorizontalMovement(ref velocity);
        ResolveVerticalMovement(ref velocity);

        Vector2 startingPosition = _rigidbody.position;

        startingPosition = ClampToPixelPerfectUnits(startingPosition, GameManager.instance.PixelsPerUnit);
        velocity = ClampToPixelPerfectUnits(velocity, GameManager.instance.PixelsPerUnit);

        _rigidbody.MovePosition(startingPosition + velocity);
    }

    private Vector2 ClampToPixelPerfectUnits(Vector2 vector, int pixelsPerUnit)
    {
        return PixelPerfectClamp.ClampVector2ToPixelUnit(vector, pixelsPerUnit);
    }

    private void ResolveVerticalMovement(ref Vector2 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float raylength = Mathf.Abs(velocity.y) + CollisionPadding;

        for (int i = 0; i < VerticalRaysAmount; i++)
        {
            Vector2 rayOrigin = (direction == -1) ? RayCorners.bottomLeft : RayCorners.topLeft;
            rayOrigin += Vector2.right * (_verticalRaySpace * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, raylength, _boundsLayer);

            if (hit)
            {
                velocity.y = (hit.distance - CollisionPadding) * direction;
                raylength = hit.distance;

                if (direction == -1)
                    CollidingBottom = true;
                else
                    CollidingTop = true;
            }
        }
    }

    private void ResolveHorizontalMovement(ref Vector2 velocity)
    {
        float direction = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + CollisionPadding;

        for (int i = 0; i < HorizontalRaysAmount; i++)
        {
            Vector2 rayOrigin = (direction == -1) ? RayCorners.bottomLeft : RayCorners.bottomRight;
            rayOrigin += Vector2.up * (_horizontalRaySpace * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayLength, _boundsLayer);

            if (hit)
            {
                velocity.x = (hit.distance - CollisionPadding) * direction;
                rayLength = hit.distance;

                if (direction == -1)
                    CollidingLeft = true;
                else
                    CollidingRight = true;
            }
        }
    }

    private void ResetCollisionStates()
    {
        CollidingTop = false;
        CollidingBottom = false;
        CollidingLeft = false;
        CollidingRight = false;
    }

    private Bounds GetBounds(BoxCollider2D collider)
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(CollisionPadding * -2);

        return bounds;
    }

    private void UpdateRaycastCorners()
    {
        Bounds bounds = GetBounds(_collider);

        RayCorners.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        RayCorners.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        RayCorners.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        RayCorners.topRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = GetBounds(_collider);

        _horizontalRaySpace = bounds.size.y / (HorizontalRaysAmount - 1);
        _verticalRaySpace = bounds.size.x / (VerticalRaysAmount - 1);
    }
}
