
public interface IDamageable
{
    int CurrentHitpoints { get; set; }

    bool CanTakeDamage { get; set; }

    void TakeDamage(int damage);

    void Die();
}
