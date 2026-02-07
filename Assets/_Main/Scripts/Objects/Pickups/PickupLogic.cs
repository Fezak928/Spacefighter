using UnityEngine;

public class PickupLogic : BaseMovementController
{
    [SerializeField] private CorePickupDataSO _pickupData;
    [SerializeField, Range(0f, 1f)] private float _threshold = 0.25f;

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
        if (IsObjectBelowCameraView() && _camera != null)
            ReturnToPool();

        Vector2 velocity = Vector2.down * _pickupData.MovementSpeed;
        Move(velocity * Time.fixedDeltaTime);
    }

    private void ReturnToPool()
    {
        ObjectPoolingManager.ReturnObjectToPool(gameObject);
    }

    private bool IsObjectBelowCameraView()
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < -_threshold)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            _pickupData.OnTriggerEnterLogic(player);
            ReturnToPool();
        }
    }
}
