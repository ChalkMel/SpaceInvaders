using ScoreSystem;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;
  private ScoreService _scoreService;

  public void Initialize(ScoreService scoreService)
  {
    _scoreService = scoreService;
    _scoreService.OnScoreChanged += DrawScore;
    DrawScore(_scoreService.Score);
  }

  private void DrawScore(int score)
  {
    if (scoreText != null)
      scoreText.text = $"Score: {score}";
  }

  private void OnDestroy()
  {
    if (_scoreService != null)
      _scoreService.OnScoreChanged -= DrawScore;
  }
}