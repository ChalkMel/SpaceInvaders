using UnityEngine;

namespace ShootingSystem
{
  public class Cover : DamageableEntity
  {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] stageSprites;

    protected override void Awake()
    {
      base.Awake();
      if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
      RefreshSprite();
      HealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
      HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int hp) => RefreshSprite();

    private void RefreshSprite()
    {
      if (spriteRenderer == null || stageSprites == null || stageSprites.Length == 0) return;
      int index = Mathf.Clamp(MaxHealth - Health, 0, stageSprites.Length - 1);
      spriteRenderer.sprite = stageSprites[index];
    }

    protected override void OnDeath()
    {
      Destroy(gameObject);
    }
  }
}