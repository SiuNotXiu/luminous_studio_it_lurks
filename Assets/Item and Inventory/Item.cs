using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // find item properties with scriptableobjects
    [SerializeField] private CollectedScrapPaper paper;
    [SerializeField] private CollectedRnU upgrade;

    public ItemData ItemData => itemData;
    public string ItemName => itemData != null ? itemData.itemName : "";
    public string ItemTag => itemData != null ? itemData.itemTag : "";
    public Sprite Sprite => itemData != null ? itemData.itemSprite : null;
    public Sprite DroppedSprite => itemData != null ? itemData.droppedSprite : null;
    public Sprite SelectedSprite => itemData != null ? itemData.selectedSprite : null;


    private InventoryController inventoryController;
    public int ScrapPaperId;//start from 1 ,, it also use for unlock upgrade paper
    public static bool isPlayerInRange = false;

    [HideInInspector] private GameObject[] object_landmark;
    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas")?.GetComponent<InventoryController>();
    }


    public void Item_Scrap_Check()
    {
        Debug.Log("Sick brooooooo");
        if (gameObject.CompareTag("ScrapPaper") && paper != null)
        {
            paper.CollectScrapPaper(ScrapPaperId);
            Debug.Log("1");
            Destroy(gameObject);
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

        }
        else if (!inventoryController.IsInventoryFull() && itemData != null)
        {
            Debug.Log("Store check");
            if (gameObject.CompareTag("PerksItem"))
            {
                upgrade.CollectedUpgrade(ScrapPaperId);
            }
            inventoryController.AddItem(itemData);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full bro stapt");
        }
    }

    public void SetItemData(ItemData data)
    {
        itemData = data;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemData != null)
        {
            spriteRenderer.sprite = itemData.itemSprite;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
            InventoryController.item = gameObject.GetComponent<Item>();
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
