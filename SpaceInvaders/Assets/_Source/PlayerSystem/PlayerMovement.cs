using UnityEngine;

namespace PlayerSystem
{
  public class PlayerMovement : MonoBehaviour
  {
    private Rigidbody2D _rb;
    private float _speed;
    public void Init(float speed, Rigidbody2D rb)
    {
      _speed = speed;
      _rb = rb;
    }
    public void Move(Vector2 movement)
    {
      if(_rb == null) Debug.LogError("_rb == null");
      Vector2 velocity = movement * _speed;
      velocity.y = _rb.linearVelocity.y;
      _rb.linearVelocity = velocity;
    }
  }
}