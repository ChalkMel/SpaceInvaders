using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySystem
{
  public class EnemyShootingController : MonoBehaviour
  {
    [SerializeField] private EnemyFactory factory;
    [SerializeField] private CoroutineRunner runner;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;

    private Coroutine _loop;
    private readonly List<Enemy> _canShoot = new();

    public void Initialize(EnemyFactory f, CoroutineRunner r)
    {
      factory = f;
      runner = r;
      _loop = runner.StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
      while (true)
      {
        yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

        var shooter = PickRandomShooter();
        if (shooter != null)
          shooter.Shoot();
      }
    }
    
    private Enemy PickRandomShooter()
    {
      _canShoot.Clear();
      var enemies = factory.Enemies;

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

    private static bool HasEnemyBelowInColumn(Enemy candidate, List<Enemy> enemies)
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