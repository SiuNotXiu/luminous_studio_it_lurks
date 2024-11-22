using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpacityForNotice : MonoBehaviour
{
    public TextMeshProUGUI Notice;
    public float fadeSpeed = 1f; //speed of the fade transition
    public float minAlpha = 0.25f;
    public float maxAlpha = 1f;    
    private bool fadingOut = true; 

    private void Start()
    {
        StartCoroutine(ChangeTextOpacity());
    }

    private IEnumerator ChangeTextOpacity()
    {
        while (true)
        {
            Color color = Notice.color;

            if (fadingOut)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                if (color.a <= minAlpha)
                {
                    color.a = minAlpha;
                    fadingOut = false; 
                }
            }
            else
            {
                color.a += fadeSpeed * Time.deltaTime;
                if (color.a >= maxAlpha)
                {
                    color.a = maxAlpha; 
                    fadingOut = true; 
                }
            }

            Notice.color = color;

            yield return null; 
        }
    }
}
