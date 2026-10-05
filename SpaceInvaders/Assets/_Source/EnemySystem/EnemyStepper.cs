using System.Collections;
using UnityEngine;

namespace EnemySystem
{
  public class EnemyStepper : MonoBehaviour
  {
    [SerializeField] private float interval = 1f;
    [SerializeField] private Vector2 stepRange = new Vector2(0.5f, 0.5f);
    [SerializeField] private float stepDown = 0.5f;

    private EnemyFactory _factory;
    private CoroutineRunner _runner;
    private int _direction = 1;
    private Coroutine _stepperCoroutine;

    public void Initialize(EnemyFactory factory, CoroutineRunner runner)
    {
      _factory = factory;
      _runner = runner;
      _stepperCoroutine = _runner.StartCoroutine(StepLoop());
    }

    private IEnumerator StepLoop()
    {
      while (true)
      {
        yield return new WaitForSeconds(interval);
        Move();
      }
    }

    private void Move()
    {
      if (_factory.Enemies.Count == 0) return;

      bool needStepDown = false;
      foreach (var enemy in _factory.Enemies)
      {
        float nextX = enemy.Position.x + stepRange.x * _direction;
        if (Mathf.Abs(nextX) > 5f)
        {
          needStepDown = true;
          break;
        }
      }

      if (needStepDown)
      {
        _direction *= -1;
        foreach (var enemy in _factory.Enemies)
          enemy.transform.position += Vector3.down * stepDown;
      }
      else
      {
        foreach (var enemy in _factory.Enemies)
          enemy.transform.position += Vector3.right * stepRange.x * _direction;
      }
    }

    private void OnDestroy()
    {
      if (_stepperCoroutine != null)
        _runner?.StopCoroutine(_stepperCoroutine);
    }
  }
}