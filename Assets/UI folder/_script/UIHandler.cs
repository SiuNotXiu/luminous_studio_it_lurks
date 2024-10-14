using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour
{
    private ItemSlot itemSlot;

    private void Start()
    {
        // Find the ItemSlot component if this script is attached to the same GameObject
        itemSlot = GetComponent<ItemSlot>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            // Check if the active dropdown menu is open
            if (itemSlot.activeDropdownMenu != null)
            {
                // Get the pointer position
                Vector2 pointerPosition = Input.mousePosition;
                // Check if the click is outside the dropdown menu and the item slot
                if (!RectTransformUtility.RectangleContainsScreenPoint(itemSlot.activeDropdownMenu.GetComponent<RectTransform>(), pointerPosition) &&
                    !RectTransformUtility.RectangleContainsScreenPoint(itemSlot.itemSlot, pointerPosition))
                {
                    itemSlot.HideDropdownMenu(); // Hide dropdown menu if clicked outside
                }
            }
        }
    }
}
