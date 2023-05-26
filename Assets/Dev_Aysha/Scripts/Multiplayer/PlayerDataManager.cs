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

        GetComponent<PlayerBoy>().nameText.text = GetComponent<PhotonView>().Owner.NickName;
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
    public void BigAndSmallBlindSetter()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PlayerBoy>().bigBlindBool = true;
            Debug.Log("You are the big blind");
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 3 && GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PlayerBoy>().smallBlindBool = true;
            Debug.Log("You are the small blind");
        }
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
        }
        else
        {
            OnlineMultiplayerManager.instance.currentTurnIndex++;
        }
    }

    [PunRPC]
    public void CallFunction()
    {
        GetComponent<PlayerBoy>().PlayerTurn();
        Debug.Log( "Calling from RPC -> " + Dealer.instance.currentBet);

        OnlineMultiplayerManager.instance.totalBetOverNetwork += Dealer.instance.currentBet;
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
            Debug.Log("Player " + PhotonNetwork.LocalPlayer.ActorNumber + " turn");
            OnlineMultiplayerManager.instance.radialSliderButton.SetActive(true);
            OnlineMultiplayerManager.instance.foldButton.SetActive(true);
        }
        else
        {
            OnlineMultiplayerManager.instance.radialSliderButton.SetActive(false);
            OnlineMultiplayerManager.instance.foldButton.SetActive(false);
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
