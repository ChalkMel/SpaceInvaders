using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSystem
{
  public class HealthView : MonoBehaviour
  {
    [SerializeField] private Image iconPrefab;
    [SerializeField] private Transform container;

    private readonly List<Image> _icons = new();

    public void Initialize(int maxHealth)
    {
      for (int i = 0; i < maxHealth; i++)
        _icons.Add(Instantiate(iconPrefab, container));
    }

    public void SetHealth(int hp)
    {
      for (int i = 0; i < _icons.Count; i++)
        _icons[i].enabled = i < hp;
    }
  }
}