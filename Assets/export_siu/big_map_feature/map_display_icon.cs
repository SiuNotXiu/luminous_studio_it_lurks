using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_display_icon : MonoBehaviour
{
    //this script is used by map icon that need to reveal only
    //player that always exist on map don't need
    //same to the material map_icon_dissolve_appear

    [SerializeField] private trigger_map_ui script_trigger_map_ui;
    [HideInInspector] private GameObject object_big_map_icon;
    [HideInInspector] private bool hasTriggered = false;

    [SerializeField] private Material material_map_icon_dissolve_appear;
    [HideInInspector] private static Material material;
    [HideInInspector] private Material material_current;
    [HideInInspector] private float dissolved_time = 0f;
    [HideInInspector] private float dissolved_duration = 0.9f;

    private void OnValidate()
    {
        if (script_trigger_map_ui == null)
        {
            script_trigger_map_ui = GameObject.Find("canvas_big_map").GetComponent<trigger_map_ui>();
        }
        //material_current = transform.Find("big_map_icon").gameObject.GetComponent<SpriteRenderer>().material;
        if (material_map_icon_dissolve_appear != null)
        {
            //Debug.Log(gameObject.name + " > material_map_icon_dissolve_appear > " + material_map_icon_dissolve_appear);
            material = material_map_icon_dissolve_appear;
            transform.Find("big_map_icon").gameObject.GetComponent<SpriteRenderer>().material = material;
            //Debug.Log(gameObject.name + " > material > " + map_display_icon.material);
        }
        if (material_map_icon_dissolve_appear == null)
        {
            //Debug.Log(gameObject.name + " > map_display_icon.material > " + material);
            material_map_icon_dissolve_appear = material;
        }
    }
    private void Start()
    {
        object_big_map_icon = transform.Find("big_map_icon").gameObject;
    }

    public void display_icon_on_map()
    {
        script_trigger_map_ui.open_map();
        StartCoroutine(delay_display_icon());
    }


    private IEnumerator delay_display_icon()
    {
        //yield return new WaitForSeconds(0.2f);
        // Code to execute after the delay

        object_big_map_icon.SetActive(true);
        while (transform.Find("big_map_icon").gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_dissolve_amount") < 1f)
        {
            //Debug.Log(gameObject.name + " revealling icon");
            if (dissolved_time < dissolved_duration)
            {
                dissolved_time += Time.deltaTime;
            }
            else
            {
                dissolved_time = dissolved_duration;
            }
            transform.Find("big_map_icon").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_dissolve_amount", dissolved_time / dissolved_duration);
            yield return null;
        }
        //yield break;
    }
}
