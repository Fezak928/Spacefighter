using UnityEngine;

public class PlayerController : MonoBehaviour, IMovable
{
    [SerializeField]
    private InputReaderSO _input;

    [SerializeField]
    private PlayerDataSO _playerData;

    public Vector2 Velocity { get; set; }

    private Vector2 _movementInput;

    private CollisionAndMovementController _movementController;

    #region Initialization

    private void OnEnable()
    {
        if (_input == null)
            return;

        _input.MovementEvent += OnMove;
    }

    private void OnDisable()
    {
        if (_input == null)
            return;

        _input.MovementEvent -= OnMove;
    }

    private void Awake()
    {
        _movementController = GetComponent<CollisionAndMovementController>();
    }

    #endregion

    private void FixedUpdate()
    {
        HandleVelocity(_movementInput);

        _movementController.Move(Velocity * Time.fixedDeltaTime);
    }

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

    private void OnPrimaryFire()
    {

    }

    private void OnSecondaryFire()
    {

    }
}
