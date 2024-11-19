using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite_creator_mask_monster : MonoBehaviour
{

    [SerializeField] public Material material; // Assign the material with the color and alpha
    private int textureSize = 256; // Size of the texture
    [SerializeField] public SpriteRenderer spriteRenderer; // SpriteRenderer to apply the generated sprite
    [SerializeField] public SpriteMask spriteMask; // SpriteRenderer to apply the generated sprite

    private void Start()
    {
        if (material != null && spriteRenderer != null)
        {
            // Generate the Texture2D from material RGBA
            Texture2D texture = GenerateTextureFromMaterialRGBA(material, textureSize, textureSize);

            // Create a Sprite from the Texture2D
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Assign the sprite to the SpriteRenderer
            //spriteRenderer.sprite = sprite;
            spriteMask.sprite = sprite;
        }
        else
        {
            Debug.LogError("Material or SpriteRenderer is not assigned.");
        }
    }

    private Texture2D GenerateTextureFromMaterialRGBA(Material material, int width, int height)
    {
        // Check if the material has a _Color property
        if (!material.HasProperty("_Color"))
        {
            Debug.LogError("The material does not have a '_Color' property.");
            return null;
        }

        // Get the RGBA color from the material
        Color materialColor = material.GetColor("_Color");

        // Create a new Texture2D
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // Fill the texture with the material's RGBA color
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = materialColor;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
