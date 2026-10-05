using System;

namespace ShootingSystem
{
  public interface IDamageable
  {
    int Health { get; }
    int MaxHealth { get; }
    bool IsAlive { get; }

    void TakeDamage(int damage);
  }
}