using EnemySystem;
using ScoreSystem;
using UnityEngine;

namespace Core
{
  public class EnemyScoreHandler : MonoBehaviour
  {
    private EnemyRegistry _registry;
    private ScoreService _scoreService;
    
    private void OnDisable() => _registry.Unregistered -= OnEnemyRemoved;

    public void Initialize(ScoreService scoreService, EnemyRegistry registry)
    {
      _registry = registry;
      _scoreService = scoreService;
      _registry.Unregistered += OnEnemyRemoved;
    }
    private void OnEnemyRemoved(Enemy enemy)
    {
      if (enemy != null) _scoreService.Add(enemy.Score);
    }
  }
}