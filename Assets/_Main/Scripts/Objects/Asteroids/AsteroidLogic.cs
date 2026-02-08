using UnityEngine;

public class AsteroidLogic : BaseMovementController, IDamageable
{
    [SerializeField] private AsteroidDataSO _asteroidData;
    [SerializeField, Range(0f, 1f)] private float _threshold = 0.25f;
    public int CurrentHitpoints { get; set; }
    public bool CanTakeDamage { get; set; }

    private Camera _camera;

    private void OnEnable()
    {
        if (_camera == null)
        {
            _camera = GameManager.Instance.PixelPerfectCamera;
        }

        CurrentHitpoints = _asteroidData.HitPoints;
    }

    private void FixedUpdate()
    {
        if(IsObjectBelowCameraView() && _camera != null)
        {
            ReturnToPool();
        }

        Vector2 velocity = Vector2.down * _asteroidData.MovementSpeed;
        Move(velocity * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHitpoints <= 0)
            return;

        CurrentHitpoints -= damage;

        if (CurrentHitpoints <= 0)
            Die();
    }

    public void Die()
    {
        GameManager.Instance.UpdateScore(_asteroidData.ScoreValue);
        AudioManagerSO.PlaySFX(_asteroidData.clips, transform.position, 1f);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CurrentHitpoints = 0;

            IDamageable player = collision.gameObject.GetComponent<IDamageable>();

            if (player.CanTakeDamage)
            {
                CameraEffects.Instance.PerformHitstop(_asteroidData.HitStopDuration);
                CameraEffects.Instance.PerformCameraShake(_asteroidData.HitStopDuration, _asteroidData.ShakeMagnitude);
            }
            else
                AudioManagerSO.PlaySFX(_asteroidData.clips, transform.position, 1f);

            player.TakeDamage(_asteroidData.Damage);
            ReturnToPool();
        }
    }

    private bool IsObjectBelowCameraView()
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);

        if(viewportPosition.y < -_threshold)
            return true;

        return false;
    }
}
