using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class PlayerDataManager : MonoBehaviourPunCallbacks
{
    public static PlayerDataManager instance;
    public PhotonView photonView;
    public GameObject playerPrefab;
    public int playerBet = 0;

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
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        if (OnlineMultiplayerManager.instance.imDealer)
            photonView.RPC("SetMyTurnRPC", RpcTarget.Others, false);
        photonView.RPC("TurnBoolTester", RpcTarget.All);
    }

    [PunRPC]
    public void CallFunction()
    {
        GetComponent<PlayerBoy>().PlayerTurn();
        Debug.Log( "Calling from RPC -> " + Dealer.instance.currentBet);

        GetComponent<PlayerBoy>().totalBetOverNetwork += Dealer.instance.currentBet;
        playerBet += Dealer.instance.currentBet;
        Debug.Log("Total Bet Over Network ->" + GetComponent<PlayerBoy>().totalBetOverNetwork);
        GetComponent<PlayerBoy>().photonView.RPC("UpdateUITextsRPC", RpcTarget.All, GetComponent<PlayerBoy>().BetChips.ToString());

    }

    [PunRPC]
    public void TurnBoolTester()
    {
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
            photonView.RPC("SetMyTurnRPC", RpcTarget.Others, true);
        }
        else
        {
            GetComponent<PlayerBoy>().myTurn = false;
            OnlineMultiplayerManager.instance.radialSliderButton.SetActive(false);
            OnlineMultiplayerManager.instance.foldButton.SetActive(false);
        }

    }


    [PunRPC]
    public void SetMyTurnRPC(bool turn)
    {
        Debug.LogError("Before Set My Turn RPC -->");
        GetComponent<PlayerBoy>().myTurn = turn;
        Debug.LogError("After Set My Turn RPC -->");
    }

    void InitPlayer()
    {
        Debug.Log("Init Player");
        foreach (var player in PhotonNetwork.PlayerList) // 2 Players Room
        {
            Debug.Log(player.NickName+":"+player.UserId);
            Debug.Log("local id: " + PhotonNetwork.LocalPlayer.UserId);
            //if (player.IsMasterClient)
            if(PhotonNetwork.LocalPlayer.UserId.Equals(player.UserId)) //Set 1st player as Dealer
            {
                OnlineMultiplayerManager.instance.Dealer.gameObject.SetActive(true);
                OnlineMultiplayerManager.instance.imDealer = true;

            }
            else
            {
                break;
            }
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
