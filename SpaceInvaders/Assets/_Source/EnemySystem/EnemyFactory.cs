using System;
using System.Collections.Generic;
using UnityEngine;
using ShootingSystem;

namespace EnemySystem
{
  public class EnemyFactory : MonoBehaviour
  {
    [Header("Layout")]
    [Tooltip("Индекс 0 — верхний ряд. Каждый элемент — тип врага в ряду.")]
    [SerializeField] private List<EnemyData> rows = new();
    [SerializeField] private Transform spawnRoot;

    [SerializeField] private ScoreService scoreService;

    public List<Enemy> Enemies { get; } = new List<Enemy>();
    public event Action AllEnemiesDead;

    public Vector2 GridOrigin { get; private set; }
    public float SpacingX { get; private set; }
    public float SpacingY { get; private set; }

    public void CreateGrid(int columns, float spacingX, float spacingY, Vector2 origin)
    {
      GridOrigin = origin;
      SpacingX = spacingX;
      SpacingY = spacingY;

      for (int r = 0; r < rows.Count; r++)
      {
        var data = rows[r];
        if (data == null) continue;

        for (int c = 0; c < columns; c++)
        {
          var pos = origin + new Vector2(c * spacingX, -r * spacingY);
          CreateEnemy(data, pos, c, r);
        }
      }
    }

    public Enemy CreateEnemy(EnemyData data, Vector2 position, int column, int row)
    {
      var enemy = Instantiate(data.Prefab, position, Quaternion.identity, spawnRoot);
      enemy.SetGridPosition(column, row);
      enemy.SetScore(data.Score);

      Enemies.Add(enemy);
      enemy.Died += OnEnemyDied;
      return enemy;
    }

    private void OnEnemyDied(DamageableEntity entity)
    {
      if (entity is Enemy enemy)
      {
        scoreService.Add(enemy.Score);
        enemy.Died -= OnEnemyDied;
        Enemies.Remove(enemy);
        if (Enemies.Count == 0) AllEnemiesDead?.Invoke();
      }
    }
  }
}