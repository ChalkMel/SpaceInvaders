using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private InputSystem_Actions _inputSystemActions;
    
    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        Bind();
        _inputSystemActions.Enable();
    }

    private void Bind()
    {
        _inputSystemActions.Player.Shoot.performed += OnShoot;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = _inputSystemActions.Player.Move.ReadValue<Vector2>();
        playerMovement.Move(direction); 
    }

    private void OnShoot(InputAction.CallbackContext obj)
    {
        //playerMovement.Jump();
    }

    private void Expose()
    {
        _inputSystemActions.Player.Shoot.performed -= OnShoot;
    }

    private void OnDisable()
    {
        _inputSystemActions.Disable();
        Expose();
    }
}
