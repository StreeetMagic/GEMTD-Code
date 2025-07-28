using System.Collections;
using UnityEngine;

namespace Services.CoroutineRunners
{
  public interface ICoroutineRunner
  {
    Coroutine StartCoroutine(IEnumerator load);
  }
}