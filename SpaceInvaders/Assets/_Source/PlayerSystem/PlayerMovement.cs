using UnityEngine;

namespace PlayerSystem
{
  public class PlayerMovement
  {
    private Rigidbody2D _rb;
    private float _speed;
    public PlayerMovement(float speed, Rigidbody2D rb)
    {
      _speed = speed;
      _rb = rb;
    }
    public void Move(Vector2 movement)
    {
      Vector2 velocity = movement * _speed;
      velocity.y = _rb.linearVelocity.y;
      _rb.linearVelocity = velocity;
    }
  }
}