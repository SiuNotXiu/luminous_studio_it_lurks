using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_display : MonoBehaviour
{
    public GameObject[] panelsToShow;  // Array to store panels to hide
    public GameObject[] panelsToHide;  // Array to store panels to show

    private void Start()
    {
        foreach (GameObject panel in panelsToShow)
        {
            AddClickListener(panel);
        }
    }



    // Method to show specific panels
    public void ShowPanels()
    {
        // Iterate over the panelsToShow array and activate each panel
        foreach (GameObject panel in panelsToShow)
        {
            panel.SetActive(true);
        }
    }

    // Method to hide specific panels
    public void HidePanels()
    {
        // Iterate over the panelsToHide array and deactivate each panel
        foreach (GameObject panel in panelsToHide)
        {
            panel.SetActive(false);
        }
    }
    private void AddClickListener(GameObject panel)
    {
        EventTrigger trigger = panel.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = panel.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { playClick(); });
        trigger.triggers.Add(entry);

    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void playClick()
    {
        if(Audio.Instance != null)
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }
}
