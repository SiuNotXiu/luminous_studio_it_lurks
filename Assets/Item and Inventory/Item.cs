using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // find item properties with scriptableobjects
    [SerializeField] private string gateID;


    public Vector3 DefaultScale => itemData != null ? itemData.defaultScale : Vector3.one;
    public ItemData ItemData => itemData;
    public string ItemName => itemData != null ? itemData.itemName : "";
    public string ItemTag => itemData != null ? itemData.itemTag : "";
    public Sprite Sprite => itemData != null ? itemData.itemSprite : null;
    public Sprite DroppedSprite => itemData != null ? itemData.droppedSprite : null;
    public Sprite SelectedSprite => itemData != null ? itemData.selectedSprite : null;

    //assign the script
    private InventoryController inventoryController;
    private CollectedScrapPaper paper;
    private CollectedRnU upgrade;

    public int ScrapPaperId;//start from 1 ,it also use for unlock upgrade paper
    public static bool isPlayerInRange = false;

    [HideInInspector] private GameObject[] object_landmark;
    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas")?.GetComponent<InventoryController>();
        paper = GameObject.Find("Journal_Canvas")?.GetComponent<CollectedScrapPaper>();
        upgrade = GameObject.Find("Journal_Canvas")?.GetComponent<CollectedRnU>();
    }


    public void Item_Scrap_Check()
    {
        //Debug.Log("Sick brooooooo");
        if (gameObject.CompareTag("ScrapPaper") && paper != null)
        {

            paper.CollectScrapPaper(ScrapPaperId);
            //Debug.Log("1");
            pickingScrap();
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
        else if(gameObject.CompareTag("EasterEgg"))
        {
            //Debug.Log("EEg apply");
            EasterEgg.Instance.OpenEasterEgg(ScrapPaperId);
            if(ScrapPaperId == 520)
            {
                pickingEeg();
            }
        }
        else if (!inventoryController.IsInventoryFull() && itemData != null)
        {
            Debug.Log("Store check");
            if (gameObject.CompareTag("PerksItem"))
            {
                upgrade.CollectedUpgrade(ScrapPaperId);
            }
            picking();
            inventoryController.AddItem(itemData);

            if (IsKey())
            {
                Debug.Log($"Picked up key for gateID: {gateID}");
            }

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

        if (data != null)
        {
            transform.localScale = data.defaultScale;
        }
    }

    public bool IsKey()
    {
        return itemData != null && itemData.isKey;
    }

    public string GetGateID()
    {
        return itemData != null ? itemData.gateID : string.Empty;
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

    #region Sound effect
    private void picking()
    {
        if (Audio.Instance != null)
        {
            if (itemData.name == "Ginseng" || itemData.name == "Yarrow" || itemData.name == "Stick")
            {
                Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.Herb_Stick_Pickup, Audio.Instance.SFXSource, 0.3f, 0.9f);
            }
            else
            {
                Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.Item_Pickup, Audio.Instance.SFXSource);
            }
        }
    }

    private void pickingScrap()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.PencilWriting, Audio.Instance.SFXSource);
        }
    }

    private void pickingEeg()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.OptionalScrapPaperPickup, Audio.Instance.SFXSource);
        }
    }


    #endregion

}
