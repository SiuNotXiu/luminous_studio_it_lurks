using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing2 : MonoBehaviour
{
    public bool cornfield { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cornfield = true;
        }
    }

    private void OnTriggerExited2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cornfield = false;
            Debug.Log("exit");
        }

        Debug.Log("exit");
    }
}
