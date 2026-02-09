using UnityEngine;

public class AsteroidLogic : BaseMovementController, IDamageable
{
    [SerializeField] private AsteroidDataSO _asteroidData;
    [SerializeField, Range(0f, 1f)] private float _threshold = 0.25f;
    [SerializeField] private ScorePopup _scorePopup;
    public int CurrentHitpoints { get; set; }
    public bool CanTakeDamage { get; set; }

    private float _movementSpeed;

    private Camera _camera;

    private void OnEnable()
    {
        if (_camera == null)
        {
            _camera = GameManager.Instance.PixelPerfectCamera;
        }

        CurrentHitpoints = _asteroidData.HitPoints;

        _movementSpeed = Random.Range(_asteroidData.MovementSpeed - _asteroidData.MovementSpeedChange, _asteroidData.MovementSpeed + _asteroidData.MovementSpeedChange);
    }

    private void FixedUpdate()
    {
        if(IsObjectBelowCameraView() && _camera != null)
        {
            ReturnToPool();
        }

        Vector2 velocity = Vector2.down * _movementSpeed;
        Move(velocity * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHitpoints <= 0)
            return;

        AudioManagerSO.PlaySFX(_asteroidData.clips, transform.position, 1f);
        CurrentHitpoints -= damage;

        if (CurrentHitpoints <= 0)
            Die();
    }

    public void Die()
    {
        ScorePopup popup = ObjectPoolingManager.SpawnObject(_scorePopup, transform.position, Quaternion.identity, ObjectPoolingManager.PoolType.VFXs);

        popup.StartCoroutine(popup.ReturnToPool(_asteroidData.ScoreValue));

        GameManager.Instance.UpdateScore(_asteroidData.ScoreValue);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CurrentHitpoints > 0)
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
