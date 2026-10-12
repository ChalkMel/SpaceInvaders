using PlayerSystem;
using UnityEngine;
using EnemySystem;

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
        [SerializeField] private float enemyOffset;
        [SerializeField] private int enemyCount;

        [Header("Score")]
        [SerializeField] private ScoreService scoreService;
        [SerializeField] private ScoreView scoreView;

        [Header("Infrastructure")]
        [SerializeField] private CoroutineRunner coroutineRunner;

        private InputListener _inputListener;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            SetupPlayer();
            SetupInput();
            SetupScore();
            SetupEnemies();
        }

        private void SetupPlayer()
        {
            healthView.Initialize(player.MaxHealth);
            healthView.SetHealth(player.Health);
            player.HealthChanged += healthView.SetHealth;
        }

        private void SetupInput()
        {
            _inputListener = gameObject.AddComponent<InputListener>();
            _inputListener.Initialize(player.GetPlayerMovement(), playerShooter);
        }

        private void SetupScore()
        {
            scoreView.Initialize(scoreService);
        }

        private void SetupEnemies()
        {
            enemyStepper.Initialize(enemyFactory, coroutineRunner);
            
            for (int i = 0; i < enemyCount; i++)
                enemyFactory.CreateEnemy(new Vector2(i - 2, 3)); //TODO
        }

        private void OnDestroy()
        {
            if (player != null) player.HealthChanged -= healthView.SetHealth;
        }
    }
}