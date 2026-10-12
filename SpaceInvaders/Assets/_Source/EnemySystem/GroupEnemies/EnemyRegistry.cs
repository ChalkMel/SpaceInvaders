using System;
using System.Collections.Generic;
using ShootingSystem;
using UnityEngine;

namespace EnemySystem
{
  public class EnemyRegistry : MonoBehaviour
  {
    private readonly List<Enemy> _enemies = new();

    public IReadOnlyList<Enemy> Enemies => _enemies;
    public int AliveCount => _enemies.Count;
    public int InitialCount { get; private set; }

    public event Action<Enemy> Registered;
    public event Action<Enemy> Unregistered;
    public event Action AllDead;

    public void Register(Enemy enemy)
    {
      if (enemy == null || _enemies.Contains(enemy)) return;
      _enemies.Add(enemy);
      enemy.Died += OnEnemyDied;
      Registered?.Invoke(enemy);
    }

    public void SetInitialCount(int count) => InitialCount = count;

    private void OnEnemyDied(DamageableEntity entity)
    {
      if (entity is not Enemy enemy) return;
      enemy.Died -= OnEnemyDied;
      _enemies.Remove(enemy);
      Unregistered?.Invoke(enemy);
      if (_enemies.Count == 0) AllDead?.Invoke();
    }
  }
}