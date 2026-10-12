using ShootingSystem;
using UnityEngine;

public class Enemy : DamageableEntity
{
  private Shooter _shooter;
  public int Column { get; private set; }
  public int Row { get; private set; }
  public int Score { get; private set; }

  protected override void Awake()
  {
    base.Awake();
    _shooter = GetComponent<Shooter>();
  }

  public void SetGridPosition(int column, int row)
  {
    Column = column;
    Row = row;
  }

  public void SetScore(int score) => Score = score;

  public void Shoot() => _shooter?.Shoot();

  protected override void OnDeath() => Destroy(gameObject);
}