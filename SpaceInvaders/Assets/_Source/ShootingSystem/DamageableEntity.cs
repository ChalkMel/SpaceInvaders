using System;
using UnityEngine;

namespace ShootingSystem
{
  public abstract class DamageableEntity : MonoBehaviour, IDamageable
  {
    [SerializeField] private int maxHealth;
    private int _health;

    public int Health => _health;
    public int MaxHealth => maxHealth;
    public bool IsAlive => _health > 0;

    public event Action<DamageableEntity> Died;

    protected virtual void Awake() => _health = maxHealth;

    public void TakeDamage(int damage)
    {
      if (!IsAlive) return;
      _health -= damage;
      if (!IsAlive)
      {
        Died?.Invoke(this);
        OnDeath();
      }
    }

    protected abstract void OnDeath();
  }
}