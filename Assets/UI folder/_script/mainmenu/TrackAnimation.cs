using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackAnimation : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image logoImage; 


    [System.Serializable]
    public class SpriteMapping
    {
        public Sprite backgroundSprite;
        public Sprite logoSprite;
    }

    [SerializeField] private List<SpriteMapping> spriteMappings = new List<SpriteMapping>();

    private Sprite lastBackgroundSprite;

    void Start()
    {
        if (backgroundImage == null || logoImage == null)
        {
            Debug.LogError("Background Image or Logo Image is not assigned. Please assign them in the Inspector.");
            return;
        }

        lastBackgroundSprite = backgroundImage.sprite;
        UpdateLogo();
    }

    void Update()
    {

        if (backgroundImage.sprite != lastBackgroundSprite)
        {
            lastBackgroundSprite = backgroundImage.sprite; 
            UpdateLogo();
        }
    }

    void UpdateLogo()
    {
        // Find the corresponding logo sprite for the current background sprite
        foreach (var mapping in spriteMappings)
        {
            if (mapping.backgroundSprite == lastBackgroundSprite)
            {
                logoImage.sprite = mapping.logoSprite;
                return;
            }
        }
    }
}
