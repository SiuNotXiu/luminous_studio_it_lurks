using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisible : MonoBehaviour
{

    private SpriteRenderer m_renderer;
    // Start is called before the first frame update
    void Start()
    {
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_renderer.isVisible)
        {
            Debug.Log("is visible");
        }
        else
        {
            Debug.Log("is not visible");
        }

    }
}
