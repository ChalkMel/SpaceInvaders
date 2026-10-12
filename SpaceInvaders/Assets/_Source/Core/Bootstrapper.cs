using PlayerSystem;
using UnityEngine;
using EnemySystem;
using ScoreSystem;
using ShootingSystem;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Player player;
        [SerializeField] private Shooter playerShooter;
        [SerializeField] private HealthView healthView;

        [Header("Enemies")]
        [SerializeField] private EnemyScoreHandler scoreHandler;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private EnemyRegistry enemyRegistry;
        [SerializeField] private EnemyStepper enemyStepper;
        [SerializeField] private EnemyShootingController enemyShooting;

        [Header("Score")]
        private ScoreService _scoreService;
        [SerializeField] private ScoreView scoreView;

        [Header("Architecture")]
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private GameManager gameManager;

        private InputListener _inputListener;

        private void Awake()
        {
            SetupScore();
            SetupGameManager();
            SetupPlayer();
            SetupEnemies();
            SetupInput();
        }

        private void SetupPlayer()
        {
            healthView.Initialize(player.MaxHealth);
            healthView.SetHealth(player.Health);
            player.HealthChanged += healthView.SetHealth;
        }

        private void SetupScore() 
        {
            _scoreService = new ScoreService();
            scoreView.Initialize(_scoreService);
        }

        private void SetupEnemies()
        {
            scoreHandler.Initialize(_scoreService, enemyRegistry);
            enemySpawner.Initialize(enemyRegistry);
            enemySpawner.CreateGrid();
            enemyStepper.Initialize(coroutineRunner, enemyRegistry, enemySpawner);
            enemyShooting.Initialize(coroutineRunner, enemyRegistry);
        }

        private void SetupInput()
        {
            _inputListener = gameObject.AddComponent<InputListener>();
            _inputListener.Initialize(player.GetPlayerMovement(), playerShooter, gameManager);
        }

        private void SetupGameManager()
        {
            gameManager.Initialize(_scoreService);
        }

        private void OnDestroy()
        {
            if (player != null) player.HealthChanged -= healthView.SetHealth;
        }
    }
}