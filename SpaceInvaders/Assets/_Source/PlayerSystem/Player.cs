using ShootingSystem;
using UnityEngine;

namespace PlayerSystem
{
  public class Player : IDamagable
  {
    private int _health;
    private int _maxHealth;
    private bool _isAlive;

    int IDamagable.Health
    {
      get => _health;
      set => _health = value;
    }

    int IDamagable.MaxHealth
    {
      get => _maxHealth;
      set => _maxHealth = value;
    }

    bool IDamagable.IsAlive
    {
      get => _isAlive;
      set => _isAlive = value;
    }
    
    private 
  }
}