using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{

    public event System.Action<Collider2D> EnteredTrigger;

    public event System.Action<Collider2D> ExitedTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnteredTrigger?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitedTrigger?.Invoke(collision);
    }

}
