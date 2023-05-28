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

    public PlayerBoy playerBoy;

    int betComparator = 1;
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
        playerBoy = GetComponent<PlayerBoy>();
        ChipsTextEnabler();
    }

    public void ChipsTextEnabler()
    {
        for (int i = 0; i < Dealer.instance.players.Count; i++)
        {
            if (Dealer.instance.players[i].photonView.IsMine)
            {
                Dealer.instance.players[i].playerChipsText.enabled = true;
            }
        }
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
        GetComponent<PlayerBoy>().photonView.RPC("PlayerRankSetterRPC", RpcTarget.All);
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && GetComponent<PhotonView>().IsMine)
        //{
        //    GetComponent<PlayerBoy>().bigBlindBool = true;
        //    Debug.Log("You are the big blind");
        //}
        //else if(PhotonNetwork.LocalPlayer.ActorNumber == 3 && GetComponent<PhotonView>().IsMine)
        //{
        //    GetComponent<PlayerBoy>().smallBlindBool = true;
        //    Debug.Log("You are the small blind");
        //}
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
        playerBet += Dealer.instance.currentBet;
        GetComponent<PlayerBoy>().photonView.RPC("UpdateUITextsRPC", RpcTarget.All, GetComponent<PlayerBoy>().BetChips.ToString());
        GetComponent<PlayerBoy>().photonView.RPC("InitialTurnRPC", RpcTarget.All, Dealer.instance.currentBet);
        //Dealer.instance.GetComponent<PhotonView>().RPC("NextPlayerTurnRPC", RpcTarget.All);
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

    //Mine
    //public void BettingValuesSetter()
    //{
    //    Debug.Log("Betting Values Setter");
    //    playerBoy.lastBetLocalPlayer = Dealer.instance.currentBet;
    //    playerBoy.playerCurrentTotalBet += playerBoy.lastBetLocalPlayer;
    //}

    //[PunRPC]
    //public void BettingValuesSetterRPC(int lastBetLocal, int totalBetPlayer)
    //{
    //    if (photonView != null)
    //    {
    //        if (!photonView.IsMine)
    //        {
    //            Debug.Log("Betting Values Setter RPC");
    //            lastBetLocal = playerBoy.lastBetLocalPlayer;
    //            totalBetPlayer = playerBoy.playerCurrentTotalBet;
    //        }
    //    }
    //}


    public void BettingValuesSetter(ref int lastBetLocal, ref int totalBetPlayer)
    {
        if (photonView != null && photonView.IsMine)
        {
            playerBoy.lastBetLocalPlayer = Dealer.instance.currentBet;
            lastBetLocal = playerBoy.lastBetLocalPlayer;
            playerBoy.playerCurrentTotalBet += playerBoy.lastBetLocalPlayer;
            totalBetPlayer = playerBoy.playerCurrentTotalBet;
            // Send updated values to other instances
            photonView.RPC("SyncBettingValues", RpcTarget.Others, lastBetLocal, totalBetPlayer);
        }
    }

    [PunRPC]
    public void SyncBettingValues(int lastBetLocal, int totalBetPlayer)
    {
        // Update the values received from the other instances
        playerBoy.lastBetLocalPlayer = lastBetLocal;
        playerBoy.playerCurrentTotalBet = totalBetPlayer;
    }

    public void AddCoinsToPotPlayerDataManager()
    {
        betComparator = 1;
        for (int i = 1; i < Dealer.instance.players.Count; i++)
        {
            if (Dealer.instance.players[i].playerCurrentTotalBet == Dealer.instance.players[0].playerCurrentTotalBet)
            {
                betComparator++;
                Debug.Log("Bets Match");
            }
            else
            {
                Debug.Log("Bets don't match");
            }
        }

        if (betComparator == Dealer.instance.players.Count)
        {
            foreach (PlayerBoy player in Dealer.instance.players)
            {
                Debug.Log("Bets are matched, add coins to pot");
                Dealer.instance.dealerChips += player.playerCurrentTotalBet;
                player.BetChips = 0;
                player.betChipsText.text = "Bet: ";
                Dealer.instance.dealerText.text = "Pot: " + Dealer.instance.dealerChips.ToString();
            }
            photonView.RPC("AddingCoinsToPotRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void AddingCoinsToPotRPC() 
    {
        Debug.Log("Adding coins to pot through RPC");
        for (int i = 1; i < Dealer.instance.players.Count; i++)
        {
            if (Dealer.instance.players[i].playerCurrentTotalBet == Dealer.instance.players[0].playerCurrentTotalBet)
            {
                betComparator++;
                Debug.Log("Bets Match");
            }
            else
            {
                Debug.Log("Bets don't match");
            }
        }
        if (betComparator == Dealer.instance.players.Count)
        {
            foreach (PlayerBoy player in Dealer.instance.players)
            {
                Dealer.instance.dealerChips += player.playerCurrentTotalBet;
                player.BetChips = 0;
                player.betChipsText.text = "Bet: ";
                Dealer.instance.dealerText.text = "Pot: " + Dealer.instance.dealerChips.ToString();
            }
        }
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
