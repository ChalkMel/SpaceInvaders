using System;
using UnityEngine;

namespace ShootingSystem
{
  public abstract class DamageableEntity : MonoBehaviour, IDamageable, ITeam
  {
    [SerializeField] private int maxHealth;
    [SerializeField] private Team team;
    private int _health;

    public Team Team => team;
    public int Health => _health;
    public int MaxHealth => maxHealth;
    public bool IsAlive => _health > 0;

    public event Action<DamageableEntity> Died;
    public event Action<int> HealthChanged;

    protected virtual void Awake()
    {
      _health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
      if (!IsAlive) return;
      _health -= damage;
      HealthChanged?.Invoke(_health);
      if (!IsAlive)
      {
        Died?.Invoke(this);
        OnDeath();
      }
    }

    protected abstract void OnDeath();
  }
}