using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float speed;
  [SerializeField] private Vector2 range;

  private Rigidbody2D _rb;
    
  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }
  public void Move(Vector2 movement)
  {
    Vector2 velocity = movement * speed;
    velocity.y = _rb.linearVelocity.y;
    _rb.linearVelocity = velocity;
  }
  
  private void FixedUpdate()
  {
    Vector2 pos = _rb.position;
    pos.x = Mathf.Clamp(pos.x, range.x, range.y);
    _rb.position = pos;
  }
}