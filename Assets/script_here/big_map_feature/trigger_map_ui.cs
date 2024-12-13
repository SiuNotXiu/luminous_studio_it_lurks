using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trigger_map_ui : MonoBehaviour
{
    public static trigger_map_ui instance;
    [SerializeField] private GameObject big_map;

    [SerializeField] private Vector3 obmb_initial_scale;
    [HideInInspector] private big_map_scrolling script_bms;

    [SerializeField] private GameObject object_map_and_icon;
    [SerializeField] private big_map_scrolling script_big_map_scrolling;
    public GameObject maptap;

    public static bool Map_Is_Open = false;


    private void OnValidate()
    {
        if (script_big_map_scrolling == null)
        {
            script_big_map_scrolling = GameObject.Find("canvas_big_map").GetComponent<big_map_scrolling>();
        }
    }
    public void OnEnable() //reset the main value in this script
    {
        Map_Is_Open = false;
    }
    private void Start()
    {
        big_map.SetActive(false);
        maptap.SetActive(false);
        #region for mouse scrolling zoom map
        obmb_initial_scale = object_map_and_icon.transform.localScale;
        script_bms = GetComponent<big_map_scrolling>();
        #endregion
    }
    private void Awake()
    {
        // Singleton Pattern (if necessary)
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (big_map != null)
        {
            if (Input.GetKeyDown(KeyCode.M) && InventoryController.JournalOpen && EasterEgg.closingEgg)
            {
                if (big_map.activeInHierarchy == false)
                {
                    open_map();
                }
                else
                {
                    closemap();
                }
            }
            //there's a pressing ecs to close function at  InventoryController script
        }
        else
        {
            Debug.Log("big_map is null");
        }
    }

    public void open_map()
    {
        if (player_database.is_flashlight_on)
        {
            player_database.is_flashlight_on = false;
            flashlightSFX();
        }
        Map_Is_Open = true;
        big_map.SetActive(true);
        maptap.SetActive(true);
        obmb_initial_scale = object_map_and_icon.transform.localScale;

    }

    public void closemap()
    {
        Debug.Log("Did it press?");
        Map_Is_Open = false;
        #region for mouse scrolling zoom map
        object_map_and_icon.transform.localScale = obmb_initial_scale;
        script_bms.current_zoom_count = 0;
        #endregion
        maptap.SetActive(false);
        big_map.SetActive(false);
        #region for mouse drag map
        object_map_and_icon.transform.localPosition = new Vector2(0, 0);
        #endregion
    }

    #region flashlightONOFf
    private void flashlightSFX()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXPlayerBehave.Instance.Flashlight, Audio.Instance.playerFlashlight);
        }
    }
    #endregion
}
