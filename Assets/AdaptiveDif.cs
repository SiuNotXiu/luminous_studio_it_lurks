using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveDif : MonoBehaviour
{
    public bool secondPaper = false;
    public bool thridPaper = false;
    public bool adaptiveDif = false;
    [SerializeField] private GameObject ad1;
    [SerializeField] private GameObject ad2;
    
  

    private IEnumerator AdaptiveTimer()
    {
        float timer = 100f; // Timer set for 300 seconds
        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            if (secondPaper)
            {
                ad1.SetActive(true);
                adaptiveDif = true;
                Debug.Log("ad1");
                StartCoroutine(AdaptiveTimer2());
                yield break; 
            }

            elapsedTime += Time.deltaTime; 
            yield return null; 
        }

       
    }

    private IEnumerator AdaptiveTimer2()
    {
        float timer = 200f; // Timer set for 300 seconds
        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            if (thridPaper)
            {

                ad2.SetActive(true);
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

      
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(AdaptiveTimer());
        }
    }
}
