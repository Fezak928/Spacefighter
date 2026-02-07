using UnityEngine;

public class AsteroidLogicController : BaseMovementController, IMovable, IDamageable
{
    [SerializeField] private DamageableEntityDataSO _asteroidData;

    [SerializeField] private float _lifeTime = 5f; // Temporary placeholder

    public Vector2 Velocity { get; set; }
    public int CurrentHitpoints { get; set; }

    private void OnEnable()
    {
        _lifeTime = 5f;
    }

    private void FixedUpdate()
    {
        if(_lifeTime > 0f)
            _lifeTime -= Time.fixedDeltaTime;
        else
            Die();

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
}
