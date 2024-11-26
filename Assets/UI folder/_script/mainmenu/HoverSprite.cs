using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image things;
    public Sprite origin; 
    public Sprite hover;

    private void Start()
    {
        if (things != null && origin != null)
        {
            things.sprite = origin;
        }
    }
    private void OnEnable()
    {
        if (things != null && origin != null)
        {
            things.sprite = origin;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (things != null && hover != null)
        {
            things.sprite = hover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (things != null && origin != null)
        {
            things.sprite = origin;
        }
    }
}
