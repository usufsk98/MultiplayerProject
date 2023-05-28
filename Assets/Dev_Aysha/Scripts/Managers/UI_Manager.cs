using System.Diagnostics;
using System.Data.SqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Analytics;


public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
   

    public Transform uiParent;
    Stack<GameObject> currentOpenedPanels = new Stack<GameObject>();
    public List<UI_Panel> UI_PanelPrefabs;
    public GameObject openedIndependantPanel;
    public bool stopOpenningPanels = false;
    public UI_Panel lastOpenedPanelForAll;// Added By Hafiz Awais 
    public bool PanelIsAlreadyOpen = false;
    public Loader loader;

    GameObject APCSpawnPanel;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Init();
    }

    public void Init()
    {
        PlayerPrefsManager.Load();
        if (PlayerPrefsManager.IsloggedInGuestValue == true.ToString())
        {
            OpenPanel(typeof(UI_MainMenu), false);
        }
        else
            OpenPanel(typeof(UI_Login), false);
    }
    public UI_Panel OpenPanel(Type panelType, bool closeLastOpened = true)
    {
        if (stopOpenningPanels) return null;

        if (closeLastOpened && currentOpenedPanels.Count > 0)
        {
            CloseLastOpenedPanel();
        }

        GameObject newPanel = Instantiate(UI_PanelPrefabs.Find(x => x.GetComponent<UI_Panel>().GetType().Equals(panelType)).gameObject, uiParent);

        currentOpenedPanels.Push(newPanel);
        return newPanel.GetComponent<UI_Panel>();

    }


    public UI_Panel OpenIndepenedantPanel(Type panelType, bool bufferPanel = false)
    {
        if (stopOpenningPanels) return null;

        GameObject newPanel = Instantiate(UI_PanelPrefabs.Find(x => x.GetComponent<UI_Panel>().GetType().Equals(panelType)).gameObject, uiParent);

        if (bufferPanel)
            openedIndependantPanel = newPanel;
        return newPanel.GetComponent<UI_Panel>();

    }

    public void CloseLastOpenedIndependantPanel()
    {
        if (openedIndependantPanel != null)
        {
            Destroy(openedIndependantPanel);
        }
    }

    public void OpenPanelForTime(Type panelType, float time)
    {
        if (stopOpenningPanels) return;

        GameObject newPanel = Instantiate(UI_PanelPrefabs.Find(x => x.GetComponent<UI_Panel>().GetType().Equals(panelType)).gameObject, uiParent);

        Destroy(newPanel, time);
    }

    public void CloseLastOpenedPanel()
    {
        if (currentOpenedPanels.Count == 0) return;

        GameObject lastOpenedPanel = currentOpenedPanels.Pop();
        Destroy(lastOpenedPanel);

        if (currentOpenedPanels != null && currentOpenedPanels.Count > 0)
        {
            if (currentOpenedPanels.Peek() != null)
            {
                if (currentOpenedPanels.Peek().GetComponent<UI_Panel>())
                {
                    UI_Panel currentPanel = currentOpenedPanels.Peek().GetComponent<UI_Panel>();
                    lastOpenedPanelForAll = currentPanel;
                    currentPanel.ResumePanel();
                }
            }
        }

    }

    internal void CloseLastOpenedPanel(Type type, bool v)
    {
        throw new NotImplementedException();
    }

    public void CloseAllPanels()
    {
        while (currentOpenedPanels != null && currentOpenedPanels.Count != 0)
        {
            CloseLastOpenedPanel();
        }
    }

    public UI_Panel GetLastOpenedPanel()
    {
        GameObject lastOpenedPanel = null;
        if (currentOpenedPanels != null && currentOpenedPanels.Count > 0)
        {
            lastOpenedPanel = currentOpenedPanels.Peek();
        }

        return lastOpenedPanel.GetComponent<UI_Panel>();
    }

    public void CloseAPCSpawnPanel()
    {
        if (APCSpawnPanel != null)
            Destroy(APCSpawnPanel);
    }

}

