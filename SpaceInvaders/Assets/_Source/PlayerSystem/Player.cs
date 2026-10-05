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

    protected override void Awake()
    {
      base.Awake();
      _rb = GetComponent<Rigidbody2D>();
      _shooter = GetComponent<Shooter>();
    }

    public void Initialize(PlayerMovement playerMovement)
    {
      _playerMovement = playerMovement;
      _playerMovement.Init(speed, _rb);
    }

    public void Shoot() => _shooter?.Shoot();

    protected override void OnDeath()
    {
      Debug.Log("Game Over");
    }
  }
}