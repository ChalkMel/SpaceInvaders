using PlayerSystem;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private InputListener _inputListener;
    [SerializeField]private Player _player;
    private PlayerMovement _playerMovement;
    [SerializeField] private Shooter playerShooter;

    private void Awake()
    {
        _playerMovement = _player.gameObject.AddComponent<PlayerMovement>();
        _player.Initialize(_playerMovement);
        _inputListener = gameObject.AddComponent<InputListener>();
        _inputListener.Initialize(_playerMovement, playerShooter);
    }
}
