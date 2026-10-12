using EnemySystem;
using UnityEngine;

namespace Core
{
  [RequireComponent(typeof(Collider2D))]
  public class KillZone : MonoBehaviour
  {
    [SerializeField] private GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (gameManager == null || !gameManager.IsGameActive) return;

      if (other.GetComponent<Enemy>() != null)
        gameManager.TriggerLose();
    }
  }
}