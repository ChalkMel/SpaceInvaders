using PlayerSystem;
using UnityEngine;
using EnemySystem;
using Core;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Player player;
        [SerializeField] private Shooter playerShooter;
        [SerializeField] private HealthView healthView;

        [Header("Enemies")]
        [SerializeField] private EnemyFactory enemyFactory;
        [SerializeField] private EnemyStepper enemyStepper;
        [SerializeField] private EnemyShootingController enemyShooting;
        [SerializeField] private int rows;
        [SerializeField] private int cols;
        [SerializeField] private float spacingX;
        [SerializeField] private float spacingY;
        [SerializeField] private Vector2 origin;

        [Header("Score")]
        [SerializeField] private ScoreService scoreService;
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

        private void SetupScore() => scoreView.Initialize(scoreService);

        private void SetupEnemies()
        {
            enemyStepper.Initialize(enemyFactory, coroutineRunner);
            enemyShooting.Initialize(enemyFactory, coroutineRunner);
            enemyFactory.CreateGrid(rows, cols, spacingX, spacingY, origin);
        }

        private void SetupInput()
        {
            _inputListener = gameObject.AddComponent<InputListener>();
            _inputListener.Initialize(player.GetPlayerMovement(), playerShooter, gameManager);
        }

        private void SetupGameManager()
        {
            gameManager.Initialize(scoreService);
        }

        private void OnDestroy()
        {
            if (player != null) player.HealthChanged -= healthView.SetHealth;
        }
    }
}