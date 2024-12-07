using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverSpecialForQuir : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image hoverImage;
    public TextMeshProUGUI buttonText; 
    private string hoverText = "Quitter"; 
    private string originalText;

    private void Start()
    {
        if (buttonText != null)
        {
            originalText = buttonText.text;
        }

        if (hoverImage != null)
        {
            hoverImage.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.text = hoverText;
        }

        if (hoverImage != null)
        {
            hoverImage.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.text = originalText;
        }

        if (hoverImage != null)
        {
            hoverImage.enabled = false;
        }
    }
}
