using UnityEngine;

public class NukeProjectileLogic : ProjectileLogic
{
    [SerializeField] private AudioClip[] _explosionClips;
    [SerializeField, Range(1, 50)] private float _areaOfEffect = 7;
    [SerializeField] private LayerMask _affectedObjectsLayer;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _areaOfEffect, _affectedObjectsLayer);

            CameraEffects.Instance.PerformHitstop(ProjectileData.HitStopDuration);
            CameraEffects.Instance.PerformCameraShake(ProjectileData.HitStopDuration, ProjectileData.ShakeMagnitude);
            AudioManagerSO.PlaySFX(_explosionClips, transform.position, 1f);
            OnImpactEvent(colliders);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _areaOfEffect);
    }
}
