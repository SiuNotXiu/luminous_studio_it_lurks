using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite_renderer_editor_invisible : MonoBehaviour
{
    [SerializeField] private Material material_sprite_lit_default;
    [SerializeField] private Material material;

    private void OnValidate() 
    {
        GetComponent<SpriteRenderer>().material = material_sprite_lit_default;
        //invisible, alpha 0
        GetComponent<SpriteRenderer>().color = new Vector4(GetComponent<SpriteRenderer>().color.r,
            GetComponent<SpriteRenderer>().color.g,
            GetComponent<SpriteRenderer>().color.b,
            0);
    } 
    private void Start()
    {
        GetComponent<SpriteRenderer>().material = material;
        //visible, alpha 1
        GetComponent<SpriteRenderer>().color = new Vector4(GetComponent<SpriteRenderer>().color.r,
            GetComponent<SpriteRenderer>().color.g,
            GetComponent<SpriteRenderer>().color.b,
            1);
    }
}
