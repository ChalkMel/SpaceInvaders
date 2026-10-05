using ShootingSystem;
using UnityEngine;

public class Enemy : DamageableEntity
{
  private Shooter _shooter;

  public Vector2 Position => transform.position;

  protected override void Awake()
  {
    base.Awake();
    _shooter = GetComponent<Shooter>();
  }

  public void Shoot() => _shooter?.Shoot();

  protected override void OnDeath()
  {
    Destroy(gameObject);
  }
}