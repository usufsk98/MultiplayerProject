using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TableSelection : UI_Panel
{
    public void Table1()
    {
        //PhotonManager.instance.maxPlayersInRoom = 2;

        //Commented by Dev
        //PhotonManager.instance.maxPlayersInRoom = 3;
        //PhotonManager.instance.FreeForAll(PhotonManager.instance.maxPlayersInRoom);
        UI_Manager.instance.OpenPanel(typeof(UI_PlayersSelection), true);
        //SceneManager.LoadScene(SceneType.GamePlay.ToString());
        // ------- Temp Fix  ---------------//
        //UI_Manager.instance.gameObject.SetActive(false);
        // ------- Temp Fix  ---------------//
    }
    public void Back()
    {
        UI_Manager.instance.OpenPanel(typeof(UI_MainMenu), true);
    }
}
