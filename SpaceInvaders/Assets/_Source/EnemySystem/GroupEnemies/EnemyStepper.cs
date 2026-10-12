using System.Collections;
using UnityEngine;

namespace EnemySystem
{
  public class EnemyStepper : MonoBehaviour
  {
    [Header("Movement")]
    [SerializeField] private float stepDown = 0.5f;
    [SerializeField] private float edgePadding = 0.5f;
    [SerializeField] private float pauseAtEdge = 0.15f;
    [SerializeField] private float baseSpeed = 1.0f;
    [SerializeField] private float maxSpeedMultiplier = 3.0f;

    [Header("Bounds")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    
    [SerializeField] private float waveAmplitude = 0.12f;
    [SerializeField] private float waveFrequency = 2.5f;
    [SerializeField] private float waveColumnPhase = 0.7f;
    [SerializeField] private float waveRowPhase = 0f;
    
    private EnemyRegistry _registry;
    private EnemySpawner _spawner;

    private CoroutineRunner _runner;
    private Coroutine _loop;

    private Vector2 _formationPos;
    private int _direction = 1;
    private float _minWorldX, _maxWorldX;
    private float _time;

    public void Initialize(CoroutineRunner runner, EnemyRegistry registry, EnemySpawner spawner)
    {
      _runner = runner;
      _registry = registry;
      _spawner = spawner;
      _formationPos = spawner.GridOrigin;
      CacheBounds();
      _loop = _runner.StartCoroutine(Loop());
    }

    private void CacheBounds()
    {
      if (Camera.main != null)
      {
        float minX = leftLimit != null
          ? leftLimit.position.x + edgePadding
          : Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x + edgePadding;

        float maxX = rightLimit != null
          ? rightLimit.position.x - edgePadding
          : Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x - edgePadding;

        _minWorldX = minX;
        _maxWorldX = maxX;
      }
    }

    private IEnumerator Loop()
    {
      while (true)
      {
        yield return null;
        if (_registry.AliveCount == 0) continue;

        _time += Time.deltaTime;

        float speedMul = ComputeSpeedMultiplier();
        float dx = _direction * baseSpeed * speedMul * Time.deltaTime;

        if (WouldHitEdge(dx))
        {
          _direction *= -1;
          _formationPos.y -= stepDown;
          if (pauseAtEdge > 0f)
            yield return new WaitForSeconds(pauseAtEdge);
        }
        else
        {
          _formationPos.x += dx;
        }

        RepositionEnemies();
      }
    }

    private float ComputeSpeedMultiplier()
    {
      int initial = _registry.InitialCount;
      if (initial <= 0) return 1f;

      float aliveRatio = _registry.AliveCount / (float)initial;
      return Mathf.Lerp(1f, maxSpeedMultiplier, 1f - aliveRatio);
    }

    private bool WouldHitEdge(float dx)
    {
      float nextFormationX = _formationPos.x + dx;
      float leftLocalX = float.MaxValue;
      float rightLocalX = float.MinValue;

      var enemies = _registry.Enemies;
      for (int i = 0; i < enemies.Count; i++)
      {
        var e = enemies[i];
        if (e == null) continue;
        float localX = e.Column * _spawner.SpacingX;
        if (localX < leftLocalX) leftLocalX = localX;
        if (localX > rightLocalX) rightLocalX = localX;
      }

      if (Mathf.Approximately(leftLocalX, float.MaxValue)) return false;

      return nextFormationX + leftLocalX <= _minWorldX
          || nextFormationX + rightLocalX >= _maxWorldX;
    }

    private void RepositionEnemies()
    {
      var enemies = _registry.Enemies;
      for (int i = 0; i < enemies.Count; i++)
      {
        var e = enemies[i];
        if (e == null) continue;

        Vector2 pos = _formationPos + new Vector2(
          e.Column * _spawner.SpacingX,
          -e.Row * _spawner.SpacingY);

          float phase = _time * waveFrequency + 
                        e.Column * waveColumnPhase + 
                        e.Row * waveRowPhase;
          pos.y += Mathf.Sin(phase) * waveAmplitude;

        e.transform.position = pos;
      }
    }
  }
}