using System;
using UnityEngine;

public class ScoreService : MonoBehaviour
{
  public int Score { get; private set; }
  public event Action<int> OnScoreChanged;

  public void Add(int value)
  {
    Score += value;
    OnScoreChanged?.Invoke(Score);
  }
}