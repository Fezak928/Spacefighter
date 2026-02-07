
public interface IDamageable
{
    int CurrentHitpoints { get; set; }

    void TakeDamage(int damage);

    void Die();
}
