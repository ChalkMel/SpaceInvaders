using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySystem
{
  public class EnemyShootingController : MonoBehaviour
  {
    private EnemyRegistry _registry;
    private CoroutineRunner _runner;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;

    private Coroutine _loop;
    private readonly List<Enemy> _canShoot = new();

    public void Initialize(CoroutineRunner runner, EnemyRegistry  registry)
    {
      _runner = runner;
      _registry = registry;
      _loop = _runner.StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
      while (true)
      {
        yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

        var shooter = PickRandomShooter();
        if (shooter != null) shooter.Shoot();
      }
    }

    private Enemy PickRandomShooter()
    {
      _canShoot.Clear();
      var enemies = _registry.Enemies;

      for (int i = 0; i < enemies.Count; i++)
      {
        var candidate = enemies[i];
        if (candidate == null) continue;
        if (!HasEnemyBelowInColumn(candidate, enemies))
          _canShoot.Add(candidate);
      }

      if (_canShoot.Count == 0) return null;
      return _canShoot[Random.Range(0, _canShoot.Count)];
    }

    private static bool HasEnemyBelowInColumn(Enemy candidate, IReadOnlyList<Enemy> enemies)
    {
      for (int i = 0; i < enemies.Count; i++)
      {
        var other = enemies[i];
        if (other == null || other == candidate) continue;
        if (other.Column != candidate.Column) continue;
        if (other.Row > candidate.Row) return true;
      }
      return false;
    }
  }
}