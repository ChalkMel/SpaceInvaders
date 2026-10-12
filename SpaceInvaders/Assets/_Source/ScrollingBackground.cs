using UnityEngine;

  [RequireComponent(typeof(MeshRenderer))]
  public class ScrollingBackground : MonoBehaviour
  {
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private Vector2 direction = new Vector2(1f, 0f);

    private Material _material;

    private void Awake()
    {
      _material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
      _material.mainTextureOffset += direction * (speed * Time.deltaTime);
    }
  }