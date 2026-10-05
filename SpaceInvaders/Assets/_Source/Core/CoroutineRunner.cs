using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
  public Coroutine StartCoroutine(IEnumerator routine)
    => base.StartCoroutine(routine);

  public void StopCoroutine(Coroutine routine)
  {
    if (routine != null) base.StopCoroutine(routine);
  }
}