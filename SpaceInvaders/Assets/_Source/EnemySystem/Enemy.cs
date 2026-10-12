using ShootingSystem;
using UnityEngine;

namespace EnemySystem
{
  public class Enemy : DamageableEntity
  {
    private Shooter _shooter;

    public Vector2 Position => transform.position;
    public int Column { get; private set; }
    public int Row { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      _shooter = GetComponent<Shooter>();
    }

    public void Shoot() => _shooter.Shoot();
    
    public void SetGridPosition(int column, int row)
    {
      Column = column;
      Row = row;
    }

    protected override void OnDeath()
    {
      Destroy(gameObject);
    }
  }
}