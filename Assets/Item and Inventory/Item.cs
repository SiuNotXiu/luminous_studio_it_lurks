using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;

    [SerializeField] private Sprite sprite;

    private InventoryController inventoryController;
    private bool isPlayerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas").GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            inventoryController.AddItem(itemName, sprite);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isPlayerInRange = false;
        }
    }
}
