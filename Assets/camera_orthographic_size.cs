using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_orthographic_size : MonoBehaviour
{
    [SerializeField] private List<Camera> camera_normal;
    [SerializeField] private List<Camera> camera_monster_mask;

    [HideInInspector] private float targetHeight = 1080f;
    [HideInInspector] private float pixelsPerUnit = 100f;

    [HideInInspector] private float monster_mask_height = 1080f * 3;

    private void OnValidate()
    {
        for (int i = 0; i < camera_normal.Count; i++)
        {
            camera_normal[i].orthographicSize = targetHeight / (2 * pixelsPerUnit);
        }

        for (int i = 0; i < camera_monster_mask.Count; i++)
        {
            camera_monster_mask[i].orthographicSize = monster_mask_height / (2 * pixelsPerUnit);
        }
    }
}
