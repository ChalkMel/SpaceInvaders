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

    [Header("Snake wave")]
    [SerializeField] private bool useSnakeWave = true;
    [SerializeField] private float waveAmplitude = 0.12f;
    [SerializeField] private float waveFrequency = 2.5f;
    [SerializeField] private float waveColumnPhase = 0.7f;
    [SerializeField] private float waveRowPhase = 0f;

    private EnemyFactory _factory;
    private CoroutineRunner _runner;
    private Coroutine _loop;

    private Vector2 _formationPos;   // мировая позиция ячейки (Column=0, Row=0)
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
      var cam = Camera.main;
      if (cam == null) { _minWorldX = -8f; _maxWorldX = 8f; return; }
      _minWorldX = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x + edgePadding;
      _maxWorldX = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x - edgePadding;
    }

    private IEnumerator Loop()
    {
      while (true)
      {
        yield return null;
        if (_factory.Enemies.Count == 0) continue;

        _time += Time.deltaTime;
        float dx = _direction * horizontalSpeed * Time.deltaTime;

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

      if (leftLocalX == float.MaxValue) return false; // нет мобов

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