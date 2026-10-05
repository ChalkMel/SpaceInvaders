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

        [Header("Enemies")]
        [SerializeField] private EnemyFactory enemyFactory;
        [SerializeField] private EnemyStepper enemyStepper;

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
            _playerMovement = player.gameObject.AddComponent<PlayerMovement>();
            player.Initialize(_playerMovement);
            player.Died += OnPlayerDied;
        }

        private void SetupInput()
        {
            _inputListener = gameObject.AddComponent<InputListener>();
            _inputListener.Initialize(_playerMovement, playerShooter);
        }

        private void SetupScore()
        {
            scoreView.Initialize(scoreService);
        }

        private void SetupEnemies()
        {
            enemyStepper.Initialize(enemyFactory, coroutineRunner);
            
            for (int i = 0; i < 5; i++)
                enemyFactory.CreateEnemy(new Vector2(i - 2, 3)); //TODO
        }

        private void OnPlayerDied(ShootingSystem.DamageableEntity entity)
        {
            Debug.Log("Game Over!");
        }
    }
}