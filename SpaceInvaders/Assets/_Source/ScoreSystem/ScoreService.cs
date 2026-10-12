using System;
using UnityEngine;

namespace ScoreSystem
{
  public class ScoreService
  {
    public int Score { get; private set; }
    public event Action<int> OnScoreChanged;

    public void Add(int value)
    {
      Score += value;
      OnScoreChanged?.Invoke(Score);
    }
  }
}