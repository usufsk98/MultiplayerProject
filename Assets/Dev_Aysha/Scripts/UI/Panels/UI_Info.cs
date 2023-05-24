using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Info : UI_Panel
{
    public Text messageText;

    RectTransform rect;

    private void Awake()
    {
        rect = transform as RectTransform;
    }

    private void OnEnable()
    {
        if (UI_Manager.instance.lastOpenedPanelForAll != null && UI_Manager.instance.lastOpenedPanelForAll.name != "UI_GamePlay(Clone)"
            && UI_Manager.instance.PanelIsAlreadyOpen)
        {
            Debug.Log("Called Here");
            UI_Manager.instance.CloseLastOpenedPanel();
        }
    }

    private void Update()
    {
        rect.SetAsLastSibling();
    }

    public void DisplayMessage(string _msg)
    {
        messageText.text = _msg;
    }

    public void ClosePanel()
    {
        //SoundManager.Instance.PlayButtonSound(0);

        // UI_Manager.Instance.CloseLastOpenedPanel();
        Destroy(gameObject);
    }
}
