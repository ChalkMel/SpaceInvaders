using System;
using ShootingSystem;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, bulletLifeTime);
    }

    public void ShootBullet(Vector3 firePoint)
    {
        _rb.AddForce(firePoint * bulletSpeed, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<IDamagable>(out var target);
        target.TakeDamage(1); //TODO
        Destroy(gameObject);
    }
}
