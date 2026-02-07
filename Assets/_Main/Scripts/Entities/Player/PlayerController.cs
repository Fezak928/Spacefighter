using UnityEngine;

[RequireComponent(typeof(CollisionAndMovementController))]
public class PlayerController : MonoBehaviour, IMovable, IDamageable
{
    [SerializeField]
    private InputReaderSO _input;
    [SerializeField]
    private PlayerDataSO _playerData;
    private CollisionAndMovementController _movementController;

    [SerializeField] private GameObject _mainProjectile;
    [SerializeField] private Transform _projectileSpawnPosition;

    public int CurrentHitpoints { get; set; }
    private Vector2 _movementInput;
    public Vector2 Velocity { get; set; }

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

        CurrentHitpoints = _playerData.HitPoints;
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

        _movementController.ApplyVelocity(Velocity * Time.fixedDeltaTime);
    }

    #region Health


    public void Heal(int heal)
    {
        if (CurrentHitpoints + heal < _playerData.HitPoints)
            CurrentHitpoints += heal;
        else
            CurrentHitpoints = _playerData.HitPoints;

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
        
        Velocity = _playerData.MovementSpeed * direction;
    }

    private float ClampInputAxis(float inputAxis)
    {
        if(Mathf.Abs(inputAxis) >= _playerData.InputThreshold)
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
        ObjectPoolingManager.SpawnObject(_mainProjectile, _projectileSpawnPosition.position, Quaternion.identity);
        _primaryFireCooldown = _playerData.FireRate;
    }

    #endregion

    #region Nukes

    private void OnSecondaryFire()
    {
        Debug.Log("Secondary fire");
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
