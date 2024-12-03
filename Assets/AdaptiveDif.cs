using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveDif : MonoBehaviour
{
    public StartAdaptiveTimer()
    {
        StartCouroutine();
    }

    private IEnumerator AdaptiveTimer()
    {
        yield return new WaitForSeconds(300)
    }

}
