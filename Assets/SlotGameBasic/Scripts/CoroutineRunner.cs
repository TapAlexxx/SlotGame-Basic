using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public Coroutine Execute(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    public void Stop(ref IEnumerator routine)
    {
        StopCoroutine(routine);
        routine = null;
    }
}