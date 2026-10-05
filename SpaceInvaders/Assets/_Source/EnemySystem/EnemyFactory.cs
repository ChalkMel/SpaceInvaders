using System.Collections.Generic;
using UnityEngine;
using ShootingSystem;
namespace EnemySystem
{
  public class EnemyFactory : MonoBehaviour
  {
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform spawnRoot;
    
    [SerializeField] ScoreService scoreService;

    public List<Enemy> Enemies { get; } = new List<Enemy>();

    public Enemy CreateEnemy(Vector2 position)
    {
      var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, spawnRoot);
      Enemies.Add(enemy);
    
      enemy.Died += OnEnemyDied;
      return enemy;
    }

    private void OnEnemyDied(DamageableEntity entity)
    {
      
      var enemy = entity as Enemy;
      if (enemy != null)
      {
        Debug.Log("Died");
        scoreService.Add(1);
        enemy.Died -= OnEnemyDied;
        Enemies.Remove(enemy);
      }
    }
  }
}