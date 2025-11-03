using System.Collections;
using System.Collections.Generic;
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