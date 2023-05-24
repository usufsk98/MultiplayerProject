using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public PhotonView photonView;
    public GameObject playerPrefab;

    public bool myTurn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        Init();
        OnlineMultiplayerManager.instance.AddPlayer(this);
    }

    void Init()
    {
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                InitPlayer();
            }

        }
    }

    [PunRPC]
    public void DealerTagEnabler()
    {
        GetComponent<PlayerBoy>().dealerTag.SetActive(true);
    }

    [PunRPC]
    public void DealerTrue()
    {
        GetComponent<PlayerBoy>().dealerBool = true;
    }


    [PunRPC]
    public void PlayGameStarter()
    {
        StartCoroutine(OnlineMultiplayerManager.instance.Dealer.PlayGame());
    }

    [PunRPC]
    public void SendingAboutDealer()
    {
        Debug.Log(PhotonNetwork.MasterClient.NickName + " is the dealer");
    }

    [PunRPC]
    public void EndTurnRPC()
    {
        if (OnlineMultiplayerManager.instance.currentTurnIndex == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            OnlineMultiplayerManager.instance.currentTurnIndex = 1;
            OnlineMultiplayerManager.instance.textComponent.text = "Player " + OnlineMultiplayerManager.instance.currentTurnIndex +" turn";
        }
        else
        {
            OnlineMultiplayerManager.instance.currentTurnIndex++;
            OnlineMultiplayerManager.instance.textComponent.text = "Player " + OnlineMultiplayerManager.instance.currentTurnIndex + " turn";
        }
    }

    [PunRPC]
    public void TurnBoolTester()
    {
        Debug.Log(OnlineMultiplayerManager.instance.currentTurnIndex);
        //foreach (var player in PhotonNetwork.PlayerList)
        //{
        //    if (player.ActorNumber == OnlineMultiplayerManager.instance.currentTurnIndex)
        //    {
        //        OnlineMultiplayerManager.instance.turnEndButton.gameObject.SetActive(true);
        //        Debug.Log("Player " + player.ActorNumber + " turn");
        //    }
        //    else
        //    {
        //        OnlineMultiplayerManager.instance.turnEndButton.gameObject.SetActive(false);
        //    }
        //}

        if (PhotonNetwork.LocalPlayer.ActorNumber == OnlineMultiplayerManager.instance.currentTurnIndex)
        {
            OnlineMultiplayerManager.instance.turnEndButton.gameObject.SetActive(true);
            Debug.Log("Player " + PhotonNetwork.LocalPlayer.ActorNumber + " turn");
        }
        else
        {
            OnlineMultiplayerManager.instance.turnEndButton.gameObject.SetActive(false);
        }
    }

    void InitPlayer()
    {
        Debug.Log("Init Player");
        foreach (var player in PhotonNetwork.PlayerList) // 2 Players Room
        {
            Debug.Log(player.NickName);
            if (player.IsMasterClient)
                OnlineMultiplayerManager.instance.Dealer.gameObject.SetActive(true);
        }

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            //GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, OnlineMultiplayerManager.instance.playerSpawnPoints[i].transform.position, OnlineMultiplayerManager.instance.playerSpawnPoints[i].transform.rotation);
            ////GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, OnlineMultiplayerManager.instance.playerSpawnPoints[0].position, Quaternion.Euler(0, 0, 0));
            //RectTransform goRectTransform = go.GetComponent<RectTransform>();
            //OnlineMultiplayerManager.instance._localPlayerDataManager = go.GetComponent<PlayerDataManager>();

            //goRectTransform.SetParent(OnlineMultiplayerManager.instance.playerSpawnPoints[0]);
            //goRectTransform.localScale = Vector3.one;
            //goRectTransform.sizeDelta = Vector2.zero;
            //goRectTransform.anchoredPosition = Vector2.zero;
        }

    }
}
