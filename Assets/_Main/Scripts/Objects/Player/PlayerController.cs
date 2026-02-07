using UnityEngine;

[RequireComponent(typeof(CollisionAndMovementController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private InputReaderSO _input;
    [field: SerializeField] public PlayerDataSO PlayerData { get; private set; }
    private CollisionAndMovementController _movementController;

    [SerializeField] private GameObject _mainProjectile, _nukeProjectile;
    [SerializeField] private Transform _projectileSpawnPosition;

    public int CurrentHitpoints { get; set; }
    private Vector2 _movementInput;
    private Vector2 _velocity;

    private bool _shooting;
    private float _primaryFireCooldown;

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


    public void Heal(int heal)
    {
        if (CurrentHitpoints + heal < PlayerData.HitPoints)
            CurrentHitpoints += heal;
        else
            CurrentHitpoints = PlayerData.HitPoints;

        GameManager.instance.AddHPDisplayHeart(CurrentHitpoints);
    }

    public void TakeDamage(int damage)
    {
        CurrentHitpoints -= damage;

        GameManager.instance.RemoveHPDisplayHeart(CurrentHitpoints);

        if (CurrentHitpoints <= 0)
            Die();
    }

    public void Die()
    {
        Debug.Log("Dead");
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

    private void OnSecondaryFire()
    {
        if(GameManager.instance.CurrentNukeAmount > 0)
        {
            ObjectPoolingManager.SpawnObject(_nukeProjectile, _projectileSpawnPosition.position, Quaternion.identity, ObjectPoolingManager.PoolType.Projectiles);
            GameManager.instance.UseNuke();
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
}
