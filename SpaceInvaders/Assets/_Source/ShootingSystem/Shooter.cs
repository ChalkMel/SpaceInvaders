using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector3  bulletDirection;
    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        bullet.GetComponent<Bullet>().ShootBullet(bulletDirection);
    }
}
