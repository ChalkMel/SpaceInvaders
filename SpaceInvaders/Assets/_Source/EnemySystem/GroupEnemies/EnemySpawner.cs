using System.Collections.Generic;
using UnityEngine;

namespace EnemySystem
{
  public class EnemySpawner : MonoBehaviour
  {
    [Header("Layout")]
    [SerializeField] private List<EnemyData> rows = new();
    [SerializeField] private Transform spawnRoot;

    [Header("Grid")]
    [SerializeField] private int columns = 8;
    [SerializeField] private float spacingX = 1f;
    [SerializeField] private float spacingY = 1f;
    [SerializeField] private Vector2 origin;

    private EnemyRegistry _registry;

    public Vector2 GridOrigin => origin;
    public float SpacingX => spacingX;
    public float SpacingY => spacingY;

    public void Initialize(EnemyRegistry  registry)
    {
      _registry = registry;
    }
    
    public void CreateGrid()
    {
      int total = 0;

      for (int r = 0; r < rows.Count; r++)
      {
        var data = rows[r];
        if (data == null) continue;

        for (int c = 0; c < columns; c++)
        {
          var pos = origin + new Vector2(c * spacingX, -r * spacingY);
          Spawn(data, pos, c, r);
          total++;
        }
      }

      _registry.SetInitialCount(total);
    }

    public Enemy Spawn(EnemyData data, Vector2 position, int column, int row)
    {
      var enemy = Instantiate(data.Prefab, position, Quaternion.identity, spawnRoot);
      enemy.SetGridPosition(column, row);
      enemy.SetScore(data.Score);
      _registry.Register(enemy);
      return enemy;
    }
  }
}