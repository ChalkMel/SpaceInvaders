using EnemySystem;
using PlayerSystem;
using ShootingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Core
{
  public class GameManager : MonoBehaviour
  {
    [SerializeField] private Player player;
    [SerializeField] private EnemyFactory enemyFactory;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI loseText;
    
    private ScoreService _scoreService;

    public bool IsGameActive { get; private set; } = true;

    public void Initialize(ScoreService score)
    {
      _scoreService = score;
    }
    private void OnEnable()
    {
      player.Died += OnPlayerDied;
      enemyFactory.AllEnemiesDead += OnAllEnemiesDead;
    }

    private void OnDisable()
    {
      player.Died -= OnPlayerDied;
      enemyFactory.AllEnemiesDead -= OnAllEnemiesDead;
    }

    private void Start()
    {
      winPanel?.SetActive(false);
      losePanel?.SetActive(false);
    }

    private void Update()
    {
      if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        Restart();
    }

    private void OnPlayerDied(DamageableEntity _) => TriggerLose();
    private void OnAllEnemiesDead() => TriggerWin();

    public void TriggerWin()
    {
      if (!IsGameActive) return;
      IsGameActive = false;
      winPanel?.SetActive(true);
      winText.text = $"You won! Your score is {_scoreService.Score}\nYou can try again with pressing R";
      Time.timeScale = 0f;
    }

    public void TriggerLose()
    {
      if (!IsGameActive) return;
      IsGameActive = false;
      losePanel?.SetActive(true);
      loseText.text = $"You lost! Your score is {_scoreService.Score}\nYou can try again with pressing R";
      Time.timeScale = 0f;
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}