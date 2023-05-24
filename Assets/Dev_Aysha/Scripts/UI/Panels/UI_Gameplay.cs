using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UI_Gameplay : UI_Panel
{
    [SerializeField] Text userName1;
    [SerializeField] Text userName2;
    void Start()
    {
        SetUserNames();
    }

    public void SetUserNames()
    {
        //for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            userName1.text = PhotonNetwork.PlayerList[0].NickName;
            userName2.text = PhotonNetwork.PlayerList[1].NickName;
        }
    }
    public void BackToMainmenu()
    {
        PhotonManager.instance.LeaveGameRoom();
        UI_Manager.instance.OpenPanel(typeof(UI_MainMenu), true);
    }
}
