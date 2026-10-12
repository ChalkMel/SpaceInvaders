using ShootingSystem;
using UnityEngine;

namespace PlayerSystem
{
  public class Player : DamageableEntity
  {
    [SerializeField] private float speed;

    private Rigidbody2D _rb;
    private PlayerMovement _playerMovement;
    private Shooter _shooter;

    public void Awake()
    {
      base.Awake();
      _rb = GetComponent<Rigidbody2D>();
      _shooter = GetComponent<Shooter>();
      _playerMovement = new PlayerMovement(speed, _rb);
    }
    
    public PlayerMovement GetPlayerMovement() => _playerMovement; 

    public void Shoot() => _shooter?.Shoot();

    protected override void OnDeath()
    {
      Debug.Log("Game Over");
    }
  }
}