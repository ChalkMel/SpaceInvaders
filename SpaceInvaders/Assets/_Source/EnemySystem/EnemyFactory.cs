using System;
using System.Collections.Generic;
using UnityEngine;
using ShootingSystem;

namespace EnemySystem
{
  public class EnemyFactory : MonoBehaviour
  {
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform spawnRoot;
    [SerializeField] private int scorePerKill = 1;
    [SerializeField] private ScoreService scoreService;

    public List<Enemy> Enemies { get; } = new List<Enemy>();
    public event Action AllEnemiesDead;

    public void CreateGrid(int rows, int cols, float spacingX, float spacingY, Vector2 origin)
    {
      for (int r = 0; r < rows; r++)
      for (int c = 0; c < cols; c++)
      {
        var pos = origin + new Vector2(c * spacingX, -r * spacingY);
        CreateEnemy(pos, c, r);
      }
    }

    public Enemy CreateEnemy(Vector2 position, int column = -1, int row = -1)
    {
      var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, spawnRoot);
      if (column >= 0 && row >= 0)
        enemy.SetGridPosition(column, row);

      Enemies.Add(enemy);
      enemy.Died += OnEnemyDied;
      return enemy;
    }

    private void OnEnemyDied(DamageableEntity entity)
    {
      if (entity is Enemy enemy)
      {
        scoreService.Add(scorePerKill);
        enemy.Died -= OnEnemyDied;
        Enemies.Remove(enemy);
        if (Enemies.Count == 0) AllEnemiesDead?.Invoke();
      }
    }
  }
}