using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollisionAndMovementController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private InputReaderSO _input;
    [field: SerializeField] public PlayerDataSO PlayerData { get; private set; }
    private CollisionAndMovementController _movementController;

    [SerializeField] private GameObject _mainProjectile, _nukeProjectile;
    [SerializeField] private Transform _projectileSpawnPosition;

    public int ShieldHitpoints { get; private set; }
    public int CurrentHitpoints { get; set; }
    private Vector2 _movementInput;
    private Vector2 _velocity;

    private bool _shooting;
    private float _primaryFireCooldown;

    public static event UnityAction PickedUpShieldEvent;
    public static event UnityAction<int> ShieldTookDamageEvent;
    public static event UnityAction ShieldDestroyedEvent;
    public static event UnityAction<int,int> TookDamageEvent;
    public static event UnityAction<int, int> HealedEvent;
    public static event UnityAction PickedUpNukeEvent;
    public static event UnityAction UsedNukeEvent;
    public static event UnityAction PlayerDead;

    private SpriteRenderer _sprite;
    public bool CanTakeDamage { get; set; } = true;
    [SerializeField] private float _flickerDuration;

    #region Initialization

    private void OnEnable()
    {
        if (_input == null)
            return;

        _input.MovementEvent += OnMove;
        _input.PrimaryFireEvent += OnPrimaryFire;
        _input.PrimaryFireCancelledEvent += OnCancelledPrimaryFire;
        _input.SecondaryFireEvent += OnSecondaryFire;
    }

    private void OnDisable()
    {
        if (_input == null)
            return;

        _input.MovementEvent -= OnMove;
        _input.PrimaryFireEvent -= OnPrimaryFire;
        _input.PrimaryFireCancelledEvent -= OnCancelledPrimaryFire;
        _input.SecondaryFireEvent -= OnSecondaryFire;
    }

    private void Awake()
    {
        _movementController = GetComponent<CollisionAndMovementController>();
        _sprite = GetComponent<SpriteRenderer>();

        CurrentHitpoints = PlayerData.HitPoints;
    }

    #endregion

    private void Update()
    {
        Timers();

        if (_shooting && _primaryFireCooldown <= 0f)
        {
            ShootProjectile();
        }
    }

    private void FixedUpdate()
    {
        HandleVelocity(_movementInput);

        _movementController.ApplyVelocity(_velocity * Time.fixedDeltaTime);
    }

    #region Health

    public void ShieldPickedUp()
    {
        ShieldHitpoints = PlayerData.HitPoints;
        PickedUpShieldEvent?.Invoke();
    }

    public void ShieldDestroyed()
    {
        ShieldDestroyedEvent?.Invoke();
    }
    
    public bool ShieldActive()
    {
        return ShieldHitpoints > 0;
    }

    public void Heal(int heal)
    {
        if(CurrentHitpoints != PlayerData.HitPoints)
        {
            if (CurrentHitpoints + heal < PlayerData.HitPoints)
                CurrentHitpoints += heal;

            else
            {
                heal = PlayerData.HitPoints - CurrentHitpoints;
                CurrentHitpoints += heal;
            }
                

            HealedEvent?.Invoke(CurrentHitpoints, heal);
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHitpoints <= 0 || !CanTakeDamage)
            return;

        AudioManagerSO.PlaySFX(PlayerData.clips, transform.position, 1f);
        StartCoroutine(InvicibilityFrames(PlayerData.InvincibilityDuration));

        if (ShieldActive())
        {
            ShieldHitpoints -= damage;

            ShieldTookDamageEvent?.Invoke(ShieldHitpoints);

            if(ShieldHitpoints <= 0)
                ShieldDestroyedEvent?.Invoke();

            return;
        }
        CurrentHitpoints -= damage;
        
        TookDamageEvent?.Invoke(CurrentHitpoints, damage);

        if (CurrentHitpoints <= 0)
            Die();
    }

    public void Die()
    {
        PlayerDead?.Invoke();
        AudioManagerSO.PlaySFX(PlayerData.DeadSFXClips, transform.position, 1f);
        //Play SFX and VFX
        gameObject.SetActive(false);
    }

    #endregion

    #region Movement
    private void OnMove(Vector2 input)
    {
        _movementInput = input;
    }

    public void HandleVelocity(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float xDirection = ClampInputAxis(direction.x);
            float yDirection = ClampInputAxis(direction.y);

            direction = new Vector2(xDirection, yDirection).normalized;
        }
        
        _velocity = PlayerData.MovementSpeed * direction;
    }

    private float ClampInputAxis(float inputAxis)
    {
        if(Mathf.Abs(inputAxis) >= PlayerData.InputThreshold)
            return Mathf.Sign(inputAxis);

        return 0;
    }
    #endregion

    #region Main Gun
    private void OnPrimaryFire()
    {
        _shooting = true;
    }

    private void OnCancelledPrimaryFire()
    {
        _shooting = false;
    }

    private void ShootProjectile()
    {
        ObjectPoolingManager.SpawnObject(_mainProjectile, _projectileSpawnPosition.position, Quaternion.identity, ObjectPoolingManager.PoolType.Projectiles);
        _primaryFireCooldown = PlayerData.FireRate;
    }

    #endregion

    #region Nukes

    public void PickedUpNuke()
    {
        PickedUpNukeEvent?.Invoke();
    }

    private void OnSecondaryFire()
    {
        if(GameManager.Instance.CurrentNukeAmount > 0)
        {
            ObjectPoolingManager.SpawnObject(_nukeProjectile, _projectileSpawnPosition.position, Quaternion.identity, ObjectPoolingManager.PoolType.Projectiles);
            UsedNukeEvent?.Invoke();
        }
            
    }

    #endregion

    private void Timers()
    {
        if (_primaryFireCooldown > 0f)
        {
            _primaryFireCooldown -= Time.deltaTime;
        }
    }

    IEnumerator InvicibilityFrames(float duration)
    {
        CanTakeDamage = false;

        for (int i = 0; i < duration; i++)
        {
            _sprite.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(_flickerDuration);
            _sprite.color = Color.white;
            yield return new WaitForSeconds(_flickerDuration);

        }
        CanTakeDamage = true;
    }
}
