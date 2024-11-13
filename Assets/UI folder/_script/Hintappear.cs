using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hintappear : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; 
    public RectTransform hintpanel;      
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Adjust this offset to position above the head

    public GameObject hint;

    bool interactible = false;
    void Start()
    {
        hint.SetActive(false); 
    }

    private void Update()
    {
        //hint.transform.localPosition = offset;
        if (hint.activeSelf)
        {
            Reposition(); // Reposition the hint image each frame
        }

        if (interactible)
        {
            hint.SetActive(true); // Show the hint when colliding with specified objects
        }
        else
        {
            hint.SetActive(false); // Show the hint when colliding with specified objects
        }
    }

    private void Reposition()
    {
        // Calculate the world position for the hint image
        Vector3 hintPosition = playerTransform.position + offset;

        // Convert the world position to screen space
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hintPosition);

        // Set the image position to the calculated screen point
        hintpanel.position = screenPoint;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Why appear?0");
        if (collision.CompareTag("CraftItem") || collision.CompareTag("PerksItem") || collision.CompareTag("Campsite") || collision.CompareTag("ScrapPaper"))
        {
            Debug.Log(gameObject.name + " collided " + collision.gameObject.name + "collided" + this);
            //hint.SetActive(true); // Show the hint when colliding with specified objects
            interactible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactible = false;
        if (collision.CompareTag("CraftItem") || collision.CompareTag("PerksItem") || collision.CompareTag("Campsite") || collision.CompareTag("ScrapPaper"))
        {
            Debug.Log(gameObject.name + " collided " + collision.gameObject.name + "close the hint" + this);
            //hint.SetActive(false); // Hide the hint when exiting the trigger
            interactible = false;
        }
    }
}
