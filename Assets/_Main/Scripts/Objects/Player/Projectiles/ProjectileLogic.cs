using UnityEngine;

public class ProjectileLogic : BaseMovementController
{
    [SerializeField] private ProjectileDataSO _projectileData;
    [SerializeField, Range(0f, 1f)] private float _threshold = 0.01f;

    private Camera _camera;

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

        Vector2 velocity = Vector2.up * _projectileData.MovementSpeed;
        Move(velocity * Time.fixedDeltaTime);
    }

    private void ReturnToPool()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject, ObjectPoolingManager.PoolType.Projectiles);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            IDamageable asteroid = collision.gameObject.GetComponent<IDamageable>();

            asteroid.TakeDamage(_projectileData.Damage);
            ReturnToPool();
        }
    }

    private bool IsObjectAboveCameraView()
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > 1 + _threshold)
            return true;

        return false;
    }
}
