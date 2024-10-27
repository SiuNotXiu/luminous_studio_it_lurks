using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hintappear : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; 
    public RectTransform hintpanel;      
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Adjust this offset to position above the head

    public GameObject hint;

    void Start()
    {
        hint.SetActive(false); 
    }

    private void Update()
    {
        if (hint.activeSelf)
        {
            Reposition(); // Reposition the hint image each frame
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
        if (collision.CompareTag("CraftItem") || collision.CompareTag("PerksItem") || collision.CompareTag("Campsite") || collision.CompareTag("ScrapPaper"))
        {
            hint.SetActive(true); // Show the hint when colliding with specified objects
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CraftItem") || collision.CompareTag("PerksItem") || collision.CompareTag("Campsite") || collision.CompareTag("ScrapPaper"))
        {
            hint.SetActive(false); // Hide the hint when exiting the trigger
        }
    }
}
