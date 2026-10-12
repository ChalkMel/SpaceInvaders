using ShootingSystem;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private float bulletSpeed;
  [SerializeField] private float bulletLifeTime;
  [SerializeField] private int damage = 1;

  private Rigidbody2D _rb;
  private Team _ownerTeam;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
    Destroy(gameObject, bulletLifeTime);
  }

  public void ShootBullet(Vector2 direction, Team ownerTeam)
  {
    _ownerTeam = ownerTeam;
    _rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.TryGetComponent<IDamageable>(out var damageable) &&
        other.gameObject.TryGetComponent<ITeam>(out var teamOwner) &&
        teamOwner.Team != _ownerTeam)
    {
      damageable.TakeDamage(damage);
      Destroy(gameObject);
    }
  }
}