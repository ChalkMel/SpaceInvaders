namespace ShootingSystem
{
    public interface IDamagable
    {
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public bool IsAlive { get; protected set; }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
