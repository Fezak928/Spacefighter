using UnityEngine;

public class ProjectileLogic : BaseMovementController
{
    [SerializeField] protected ProjectileDataSO ProjectileData;
    [SerializeField, Range(0f, 1f)] private float _threshold = 0.01f;
    protected bool _hasHit;

    private Camera _camera;

    private void OnEnable()
    {
        if (_camera == null)
        {
            _camera = GameManager.Instance.PixelPerfectCamera;
        }

        AudioManagerSO.PlaySFX(ProjectileData.clips, transform.position, 0.5f);
        _hasHit = false;
    }

    private void FixedUpdate()
    {
        if (IsObjectAboveCameraView(_threshold) && _camera != null)
        {
            ReturnToPool();
        }

        Vector2 velocity = Vector2.up * ProjectileData.MovementSpeed;
        Move(velocity * Time.fixedDeltaTime);
    }

    protected void ReturnToPool()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject, ObjectPoolingManager.PoolType.Projectiles);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") && !IsObjectAboveCameraView(0f) && !_hasHit)
        {
            IDamageable asteroid = collision.gameObject.GetComponent<IDamageable>();
            _hasHit = true;

            if (asteroid.CurrentHitpoints == ProjectileData.Damage)
            {
                CameraEffects.Instance.PerformHitstop(ProjectileData.HitStopDuration);
            }
            CameraEffects.Instance.PerformCameraShake(ProjectileData.HitStopDuration, ProjectileData.ShakeMagnitude);
            
            OnImpactEvent(asteroid);
        }
    }

    protected void OnImpactEvent(IDamageable damageable)
    {
        damageable.TakeDamage(ProjectileData.Damage);
        ReturnToPool();
    }

    protected void OnImpactEvent(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            damageable.TakeDamage(ProjectileData.Damage);
        }
        ReturnToPool();
    }

    protected bool IsObjectAboveCameraView(float offset)
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > 1 + offset)
            return true;

        return false;
    }
}
