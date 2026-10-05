using PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Shooter _shooter;
    private InputSystem_Actions _actions;

    public void Initialize(PlayerMovement movement, Shooter shooter)
    {
        _playerMovement = movement;
        _shooter = shooter;
        _actions = new InputSystem_Actions();
        Bind();
        _actions.Enable();
    }

    private void Bind()
    {
        _actions.Player.Shoot.performed += OnShoot;
    }

    private void Unbind()
    {
        _actions.Player.Shoot.performed -= OnShoot;
    }

    private void Update()
    {
        if (_actions == null || _playerMovement == null) return;
        var dir = _actions.Player.Move.ReadValue<Vector2>();
        _playerMovement.Move(dir);
    }

    private void OnShoot(InputAction.CallbackContext ctx) => _shooter.Shoot();

    private void OnDestroy()
    {
        Unbind();
        _actions?.Disable();
    }
}
