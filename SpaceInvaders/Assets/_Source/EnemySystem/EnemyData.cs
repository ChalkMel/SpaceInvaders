using UnityEngine;

namespace EnemySystem
{
  [CreateAssetMenu(menuName = "SpaceInvaders/Enemy Data", fileName = "EnemyData")]
  public class EnemyData : ScriptableObject
  {
    [field:SerializeField] private Enemy prefab;
    [field:SerializeField] private int score;

    public Enemy Prefab => prefab;
    public int Score => score;
  }
}