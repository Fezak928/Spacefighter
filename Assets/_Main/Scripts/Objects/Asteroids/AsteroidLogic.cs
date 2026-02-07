using UnityEngine;

public class AsteroidLogic : BaseMovementController, IDamageable
{
    [SerializeField] private AsteroidDataSO _asteroidData;
    [SerializeField, Range(0f, 1f)] private float _threshold = 0.25f;
    public int CurrentHitpoints { get; set; }

    private Camera _camera;

    private void OnEnable()
    {
        if (_camera == null)
        {
            _camera = GameManager.instance.PixelPerfectCamera;
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
        CurrentHitpoints -= damage;

        if (CurrentHitpoints <= 0)
            Die();
    }

    public void Die()
    {
        GameManager.instance.UpdateScore(_asteroidData.ScoreValue);
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
            IDamageable player = collision.gameObject.GetComponent<IDamageable>();

            player.TakeDamage(_asteroidData.Damage);
            Die();
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
