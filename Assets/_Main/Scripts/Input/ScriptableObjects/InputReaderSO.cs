using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "_InputReader", menuName = "_InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions
{
    public event UnityAction<Vector2> MovementEvent;
    public event UnityAction PrimaryFireEvent;
    public event UnityAction PrimaryFireCancelledEvent;
    public event UnityAction SecondaryFireEvent;

    private Controls _controlsScript;

    private void OnEnable()
    {
        if (_controlsScript == null)
        {
            _controlsScript = new();
            _controlsScript.Player.SetCallbacks(this);
        }

        _controlsScript.Enable();
            
    }

    private void OnDisable()
    {
        _controlsScript.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (PrimaryFireEvent != null && context.started)
            PrimaryFireEvent.Invoke();

        if(PrimaryFireCancelledEvent != null && context.canceled)
            PrimaryFireCancelledEvent.Invoke();
    }

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        
        if (SecondaryFireEvent != null && context.started)
            SecondaryFireEvent.Invoke();
    }
}
