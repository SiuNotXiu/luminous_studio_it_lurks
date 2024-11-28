using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{

    public event System.Action<Collider2D> EnteredTrigger;

    public event System.Action<Collider2D> ExitedTrigger;
    //[SerializeField] public List<GameObject> object_in_range;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnteredTrigger?.Invoke(collision);
        //object_in_range.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitedTrigger?.Invoke(collision);
        //object_in_range.Remove(collision.gameObject);
    }

}
