using Core;
using PlayerSystem;
using ShootingSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Shooter _shooter;
    private InputSystem_Actions _actions;
    private GameManager _gameManagerRef;

    public void Initialize(PlayerMovement movement, Shooter shooter, GameManager gameManager)
    {
        _playerMovement = movement;
        _shooter = shooter;
        _actions = new InputSystem_Actions();
        _gameManagerRef = gameManager;
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
        if (!_gameManagerRef.IsGameActive) return;
        if (_actions == null || _playerMovement == null) return;
        var dir = _actions.Player.Move.ReadValue<Vector2>();
        _playerMovement.Move(dir);
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (!_gameManagerRef.IsGameActive) return;
        _shooter.Shoot();
    }

    private void OnDestroy()
    {
        Unbind();
        _actions?.Disable();
    }
}
