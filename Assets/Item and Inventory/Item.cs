using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Reference to the ItemData ScriptableObject
    [SerializeField] private CollectedScrapPaper paper;

    public string ItemName => itemData != null ? itemData.itemName : "";
    public string ItemTag => itemData != null ? itemData.itemTag : "";
    public Sprite Sprite => itemData != null ? itemData.itemSprite : null;

    private InventoryController inventoryController;
    public int ScrapPaperId;//start from 1
    public bool isPlayerInRange = false;

    [HideInInspector] private GameObject[] object_landmark;
    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas")?.GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (gameObject.CompareTag("ScrapPaper") && paper != null)
            {
                paper.CollectScrapPaper(ScrapPaperId);
/*                #region map icon activation
                if (object_landmark[ScrapPaperId - 1] != null)
                {
                    if (object_landmark[ScrapPaperId - 1].GetComponent<map_display_icon>() != null)
                    {
                        object_landmark[ScrapPaperId - 1].GetComponent<map_display_icon>().display_icon_on_map();
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " need map icon");
                    }
                }
                else
                {
                    Debug.Log("object_landmark[ScrapPaperId - 1] is null");
                }
                #endregion*/
                Destroy(gameObject);
            }
            else if (!inventoryController.IsInventoryFull() && itemData != null)
            {
                Debug.Log("Store check");
                inventoryController.AddItem(itemData);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full bro stapt");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }
    
}
