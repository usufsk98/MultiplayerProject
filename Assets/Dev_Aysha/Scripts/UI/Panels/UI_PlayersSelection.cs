using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayersSelection : UI_Panel
{
    [SerializeField] int minValue;
    [SerializeField] int maxValue;
    [SerializeField] Text valueText;
    [SerializeField] int val;

    public void Min()
    {
        if(val >5)
        {
            val--;
            valueText.text = val.ToString();
            PhotonManager.instance.maxPlayersInRoom = val;
        }
    }
    public void Max()
    {
        if (val < 8)
        {
            val++;
            valueText.text = val.ToString();
            PhotonManager.instance.maxPlayersInRoom = val;
        }
        
    }
    public void CreatejoinRoom()
    {
        //For now as Gameplay UI is setted for 5 .. hard code the value as 5 later will comment this 
        PhotonManager.instance.maxPlayersInRoom = 2;
        PhotonManager.instance.FreeForAll(PhotonManager.instance.maxPlayersInRoom);
        UI_Manager.instance.OpenPanel(typeof(UI_WaitingForOtherPlayers), true);
    }
}
