using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class MonoBehaiviourExtainsion
{
    public static void wait(this MonoBehaviour mono ,UnityAction callback , float delay)
    {
        mono.StartCoroutine(ExecuteAction(callback, delay));
    }

    public static IEnumerator ExecuteAction(UnityAction callback , float delay)
    {
        yield return new WaitForSeconds(delay);
        callback.Invoke();
        yield break;
    }
}
