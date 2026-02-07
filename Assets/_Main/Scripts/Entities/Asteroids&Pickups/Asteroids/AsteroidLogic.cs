using UnityEngine;

public class AsteroidLogic : BaseMovementController, IMovable, IDamageable
{
    [SerializeField] private DamageableEntityDataSO _asteroidData;

    public Vector2 Velocity { get; set; }
    public int CurrentHitpoints { get; set; }

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
        if(IsObjectBelowCameraView() && _camera != null)
        {
            Die();
        }

        HandleVelocity(Vector2.down);
    }

    public void HandleVelocity(Vector2 direction)
    {
        Velocity = direction * _asteroidData.MovementSpeed;

        Move(Velocity * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        CurrentHitpoints -= damage;

        if (CurrentHitpoints <= 0)
            Die();
    }

    public void Die()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDamage(1);
            Die();
        }
    }

    private bool IsObjectBelowCameraView()
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);

        if(viewportPosition.y < -0.25f)
        {
            return true;
        }

        return false;
    }
}
