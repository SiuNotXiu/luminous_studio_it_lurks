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
            // Generate a Texture2D from the material color and alpha
            Texture2D texture = GenerateTextureFromMaterial(material, textureSize, textureSize);

            // Convert the Texture2D to a Sprite
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Assign the sprite to the SpriteRenderer
            spriteRenderer.sprite = sprite;
            spriteMask.sprite = sprite;
        }
        else
        {
            Debug.LogError("Material or SpriteRenderer is not assigned.");
        }
    }

    private Texture2D GenerateTextureFromMaterial(Material material, int width, int height)
    {
        // Retrieve the color from the material
        Color materialColor = material.GetColor("_Color");

        // Create a Texture2D
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // Fill the texture with the material's color and alpha
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
