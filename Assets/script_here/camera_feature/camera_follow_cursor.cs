using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow_cursor : MonoBehaviour
{
    [SerializeField] private GameObject object_player;

    private void OnValidate()
    {
        if (object_player == null)
        {
            object_player = GameObject.Find("player_dont_change_name");
        }
        transform.position = new Vector3(object_player.transform.position.x,
                                         object_player.transform.position.y,
                                         object_player.transform.position.z - 100f);
    }

    // Update is called once per frame
    void Update()
    {
        //new code fix by chen jie (still cant fix the jittery buggy)
        if (InventoryController.JournalOpen && EasterEgg.closingEgg && !trigger_map_ui.Map_Is_Open)
        {
            float remember_this_float = transform.position.z;

            // Smooth interpolation with time scaling
            Vector3 targetPosition = Vector3.Lerp(transform.position,
                                                  (object_player.transform.position + get_mouse_position()) / 2,
                                                  5f * Time.deltaTime);
            transform.position = new Vector3(targetPosition.x, targetPosition.y, remember_this_float);
        }


        //old code use by hengsiu
        /*        float remember_this_float = transform.position.z;
                transform.position = (get_mouse_position() + object_player.transform.position) / 2;
                transform.position = new Vector3(transform.position.x,
                    transform.position.y,
                    remember_this_float);*/
    }
    Vector3 get_mouse_position()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // Set z to 0 for 2D
        return worldPosition;
    }
}
