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

    public int betComparator = 1;
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
        playerBoy.photonView.RPC("InitialTurnRPC", RpcTarget.All, Dealer.instance.currentBet);
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
            GameInputManager.instance.localbetValue = Dealer.instance.currentBet;
            playerBoy.lastBetLocalPlayer = Dealer.instance.currentBet;
            lastBetLocal = playerBoy.lastBetLocalPlayer;
            playerBoy.playerCurrentTotalBet += playerBoy.lastBetLocalPlayer;
            totalBetPlayer = playerBoy.playerCurrentTotalBet;
            playerBoy.betChips += playerBoy.betValueCurrent;
            // Send updated values to other instances
            photonView.RPC("SyncBettingValues", RpcTarget.Others, lastBetLocal, totalBetPlayer);
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && OnlineMultiplayerManager.instance.roundNumber == 1 && OnlineMultiplayerManager.instance.firstTime)
        {
            playerBoy.lastBetLocalPlayer = 200;
            lastBetLocal = playerBoy.lastBetLocalPlayer;
            playerBoy.playerCurrentTotalBet += playerBoy.lastBetLocalPlayer;
            totalBetPlayer = playerBoy.playerCurrentTotalBet;
            playerBoy.betChips = playerBoy.lastBetLocalPlayer;
            playerBoy.betChipsText.text = playerBoy.lastBetLocalPlayer.ToString();
            playerBoy.playerChips = 7800;
            playerBoy.playerChipsText.text = playerBoy.playerChips.ToString();
            photonView.RPC("SyncBettingValues", RpcTarget.Others, lastBetLocal, totalBetPlayer);
            //Need to remove this line as this is for testing on two players
            photonView.RPC("SettingBoolToFalse", RpcTarget.All);
            //Need to remove this line as this is for testing on two players
            playerBoy.photonView.RPC("UpdateUITextsRPC", RpcTarget.Others, playerBoy.playerCurrentTotalBet.ToString());
            EndTurnForFirstRound();
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 3 && OnlineMultiplayerManager.instance.roundNumber == 1 && OnlineMultiplayerManager.instance.firstTime)
        {
            playerBoy.lastBetLocalPlayer = 400;
            lastBetLocal = playerBoy.lastBetLocalPlayer;
            playerBoy.playerCurrentTotalBet += playerBoy.lastBetLocalPlayer;
            totalBetPlayer = playerBoy.playerCurrentTotalBet;
            playerBoy.betChips = playerBoy.lastBetLocalPlayer;
            playerBoy.betChipsText.text = playerBoy.lastBetLocalPlayer.ToString();
            playerBoy.playerChips = 7600;
            playerBoy.playerChipsText.text = playerBoy.playerChips.ToString();
            photonView.RPC("SettingBoolToFalse", RpcTarget.All);
            photonView.RPC("SyncBettingValues", RpcTarget.Others, lastBetLocal, totalBetPlayer);
            playerBoy.photonView.RPC("UpdateUITextsRPC", RpcTarget.Others, playerBoy.playerCurrentTotalBet.ToString());
            EndTurnForFirstRound();
        }
    }

    public void CheckingForWinner()
    {
        if (WinningScenerio.instance.GetWinner(Dealer.instance.players, Dealer.instance.comunityCards) == Dealer.instance.currentPlayerBoy)
        {
            Debug.Log(WinningScenerio.instance.GetWinner(Dealer.instance.players, Dealer.instance.comunityCards));
            GameInputManager.instance.winPanel.SetActive(true);
        }
        else
        {
            Debug.Log(WinningScenerio.instance.GetWinner(Dealer.instance.players, Dealer.instance.comunityCards));
            GameInputManager.instance.losePanel.SetActive(true);
        }
    }

    public void EndTurnForFirstRound()
    {
        photonView.RPC("EndTurnRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SettingBoolToFalse()
    {
        OnlineMultiplayerManager.instance.firstTime = false;
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
                Dealer.instance.currentPlayerBoy.betChips = 0;
                Dealer.instance.currentPlayerBoy.photonView.RPC("UpdateUITextsRPC", RpcTarget.All, "0");                                                                                                
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
                player.betChips = 0;
                player.playerCurrentTotalBet = 0;
                player.betChipsText.text = "0";
                Dealer.instance.dealerText.text = "Pot: " + Dealer.instance.dealerChips.ToString();
                betComparator = 1;
            }
            OnlineMultiplayerManager.instance.roundNumber++;
            if (OnlineMultiplayerManager.instance.roundNumber == 2)
            {
                Dealer.instance.PreFlop();
            }
            if (OnlineMultiplayerManager.instance.roundNumber == 2)
            {
                Dealer.instance.PreFlop();
            }
            else if (OnlineMultiplayerManager.instance.roundNumber == 3)
            {
                Dealer.instance.Flop();
            }
            else if (OnlineMultiplayerManager.instance.roundNumber == 4)
            {
                Dealer.instance.Turn();
                CheckingForWinner();
            }
            photonView.RPC("AddingCoinsToPotRPC", RpcTarget.Others);
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
                Dealer.instance.currentPlayerBoy.betChips = 0;
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
                player.betChips = 0;
                player.playerCurrentTotalBet = 0;
                player.betChipsText.text = "0";
                Dealer.instance.dealerText.text = "Pot: " + Dealer.instance.dealerChips.ToString();
                betComparator = 1;
            }
            OnlineMultiplayerManager.instance.roundNumber++;
            if (OnlineMultiplayerManager.instance.roundNumber == 2)
            {
                Dealer.instance.PreFlop();
            }
            else if (OnlineMultiplayerManager.instance.roundNumber == 3)
            {
                Dealer.instance.Flop();
            }
            else if (OnlineMultiplayerManager.instance.roundNumber == 4)
            {
                Dealer.instance.Turn();
                CheckingForWinner();
            }
        }
    }

    //public void FoldButtonClick()
    //{
    //    playerBoy.Fold();
    //    photonView.RPC("FoldButtonClickRPC", RpcTarget.All);
    //    PhotonNetwork.LeaveRoom();
    //    Debug.Log(OnlineMultiplayerManager.instance.playersList.Count);
    //}

    //[PunRPC]
    //public void FoldButtonClickRPC()
    //{

    //    if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
    //    {
    //        Debug.Log("You are the last left and you win");
    //    }

    //}



    public void FoldButtonClick()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("You are the last left and you win");
            
        }
        else
        {
            playerBoy.Fold();
            GameInputManager.instance.losePanel.SetActive(true);
            photonView.RPC("FoldButtonClickRPC", RpcTarget.All);
        }

        Debug.Log(OnlineMultiplayerManager.instance.playersList.Count);
    }

    [PunRPC]
    public void FoldButtonClickRPC()
    {
        Debug.Log("You are the only one left and you win through RPC");
        GameInputManager.instance.winPanel.SetActive(true);
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
