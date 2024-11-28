using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseMapBTN : MonoBehaviour
{
    public Canvas canvas; // Reference to your Canvas
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            DetectUIElementClick();
        }
    }
    void DetectUIElementClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // Current mouse position
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults); // Perform raycast on all UI elements

        foreach (RaycastResult result in raycastResults)
        {
            // Log the clicked UI element or perform actions
            Debug.Log("Clicked on: " + result.gameObject.name);

            // Example: Perform action on specific images
            if (result.gameObject.name == "CloseMapBTN")
            {
                Debug.Log("Clicked on an image with tag 'Image': " + result.gameObject.name);
            }
        }
    }
}
