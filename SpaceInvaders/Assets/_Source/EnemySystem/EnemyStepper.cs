using System.Collections;
using UnityEngine;

namespace EnemySystem
{
  public class EnemyStepper : MonoBehaviour
  {
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 1.5f;
    [SerializeField] private float stepDown = 0.5f;
    [SerializeField] private float edgePadding = 0.5f;
    [SerializeField] private float pauseAtEdge = 0.15f;
    [SerializeField] private float baseSpeed = 1.0f;
    [SerializeField] private float maxSpeedMultiplier = 3.0f;

    [Header("Bounds")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    [Header("Snake wave")]
    [SerializeField] private bool useSnakeWave = true;
    [SerializeField] private float waveAmplitude = 0.12f;
    [SerializeField] private float waveFrequency = 2.5f;
    [SerializeField] private float waveColumnPhase = 0.7f;
    [SerializeField] private float waveRowPhase = 0f;
    
    private EnemyFactory _factory;
    private CoroutineRunner _runner;
    private Coroutine _loop;

    private Vector2 _formationPos;
    private int _direction = 1;
    private float _minWorldX, _maxWorldX;
    private float _time;

    public void Initialize(EnemyFactory factory, CoroutineRunner runner)
    {
      _factory = factory;
      _runner = runner;
      _formationPos = factory.GridOrigin;
      CacheBounds();
      _loop = _runner.StartCoroutine(Loop());
    }

    private void CacheBounds()
    {
      float minX, maxX;

      if (leftLimit != null)
        minX = leftLimit.position.x + edgePadding;
      else
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x + edgePadding;

      if (rightLimit != null)
        maxX = rightLimit.position.x - edgePadding;
      else
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x - edgePadding;

      _minWorldX = minX;
      _maxWorldX = maxX;
    }

    private IEnumerator Loop()
    {
      while (true)
      {
        yield return null;
        if (_factory.Enemies.Count == 0) continue;

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
      int initial = _factory.InitialCount;
      if (initial <= 0) return 1f;

      int alive = _factory.Enemies.Count;
      float aliveRatio = alive / (float)initial;
      float deadRatio  = 1f - aliveRatio;

      return Mathf.Lerp(1f, maxSpeedMultiplier, deadRatio);
    }

    private bool WouldHitEdge(float dx)
    {
      float nextFormationX = _formationPos.x + dx;
      float leftLocalX = float.MaxValue;
      float rightLocalX = float.MinValue;

      foreach (var e in _factory.Enemies)
      {
        if (e == null) continue;
        float localX = e.Column * _factory.SpacingX;
        if (localX < leftLocalX) leftLocalX = localX;
        if (localX > rightLocalX) rightLocalX = localX;
      }

      if (leftLocalX == float.MaxValue) return false;

      return nextFormationX + leftLocalX <= _minWorldX
          || nextFormationX + rightLocalX >= _maxWorldX;
    }

    private void RepositionEnemies()
    {
      foreach (var e in _factory.Enemies)
      {
        if (e == null) continue;

        Vector2 pos = _formationPos + new Vector2(
          e.Column * _factory.SpacingX,
          -e.Row * _factory.SpacingY);

        if (useSnakeWave)
        {
          float phase = _time * waveFrequency
                      + e.Column * waveColumnPhase
                      + e.Row * waveRowPhase;
          pos.y += Mathf.Sin(phase) * waveAmplitude;
        }

        e.transform.position = pos;
      }
    }

    private void OnDestroy() => _runner?.StopCoroutine(_loop);
  }
}