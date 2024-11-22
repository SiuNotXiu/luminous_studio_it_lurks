using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_map_ui : MonoBehaviour
{
    [SerializeField] private GameObject big_map;

    [SerializeField] private GameObject object_big_map_background;
    [HideInInspector] private Vector2 obmb_initial_scale;
    [HideInInspector] private big_map_scrolling script_bms;

    [SerializeField] private GameObject object_map_and_icon;

    public static bool Map_Is_Open = false;

    private void Start()
    {
        big_map.SetActive(false);
        #region for mouse scrolling zoom map
        obmb_initial_scale = object_big_map_background.transform.localScale;
        script_bms = GetComponent<big_map_scrolling>();
        #endregion
    }
    // Update is called once per frame
    void Update()
    {
        if (big_map != null)
        {
            if (Input.GetKeyDown(KeyCode.M) && InventoryController.JournalOpen)
            {
                if (big_map.activeInHierarchy == false)
                {
                    open_map();
                }
                else
                {
                    Map_Is_Open = false;
                    big_map.SetActive(false);
                    #region for mouse scrolling zoom map
                    object_big_map_background.transform.localScale = obmb_initial_scale;
                    script_bms.current_zoom_count = 0;
                    #endregion
                    #region for mouse drag map
                    object_map_and_icon.transform.localPosition = new Vector2(0, 0);
                    #endregion
                }
            }
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
