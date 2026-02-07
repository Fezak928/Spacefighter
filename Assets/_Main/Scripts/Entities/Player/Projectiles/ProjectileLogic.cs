using UnityEngine;

public class ProjectileLogic : BaseMovementController, IMovable
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _damage;

    private Camera _camera;
    public Vector2 Velocity { get; set; }

    private void OnEnable()
    {
        if (_camera == null)
        {
            _camera = GameManager.instance.PixelPerfectCamera;
        }
    }

    private void FixedUpdate()
    {
        if (IsObjectAboveCameraView() && _camera != null)
        {
            ReturnToPool();
        }

        HandleVelocity(Vector2.up);
    }

    public void HandleVelocity(Vector2 direction)
    {
        Velocity = direction * _movementSpeed;

        Move(Velocity * Time.fixedDeltaTime);
    }

    private void ReturnToPool()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            AsteroidLogic asteroid = collision.gameObject.GetComponent<AsteroidLogic>();

            asteroid.TakeDamage(1);
            ReturnToPool();
        }
    }

    private bool IsObjectAboveCameraView()
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > 1.25f)
        {
            return true;
        }

        return false;
    }
}
