using UnityEngine;

namespace ShootingSystem
{
  [RequireComponent(typeof(ITeam))]
  public class Shooter : MonoBehaviour
  {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector3 bulletDirection;

    private ITeam _team;

    private void Awake()
    {
      _team = GetComponent<ITeam>();
    }

    public void Shoot()
    {
      var bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
      bullet.GetComponent<Bullet>().ShootBullet(bulletDirection, _team?.Team ?? Team.Other);
    }
  }
}