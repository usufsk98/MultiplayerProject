using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


 
public class UI_MainMenu : UI_Panel
{
    [SerializeField] Text userName;
    [SerializeField] Text chips;
    [SerializeField] List<GameObject> bottomButtons;

    private void OnEnable()
    {
        PlayerPrefsManager.Load();
        userName.text = PlayerPrefsManager.UserCodeValue;
        chips.text = PlayerPrefsManager.ChipsValue.ToString();
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefsManager.UserCodeValue;
    }
    private void Start()
    {
        Home();
    }
    public void PLOMode()
    {
        PhotonManager.instance.FreeForAll(2);
        UI_Manager.instance.OpenPanel(typeof(UI_WaitingForOtherPlayers), true);
    }
    public void HoldemMode()
    {
        UI_Manager.instance.OpenPanel(typeof(UI_TableSelection), true);
    }
    public void Profile()
    {
        UI_Manager.instance.OpenPanel(typeof(UI_Profile), false);
    }
    public void Settings()
    {
        UI_Manager.instance.OpenPanel(typeof(UI_Settings), false);
    }
    public void Home()
    {
        PlayerPrefsManager.Load();
        chips.text = PlayerPrefsManager.ChipsValue.ToString();
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            bottomButtons[i].SetActive(false);
        }
    }
}


