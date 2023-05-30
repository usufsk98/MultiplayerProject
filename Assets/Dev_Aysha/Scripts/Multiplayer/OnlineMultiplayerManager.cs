using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class OnlineMultiplayerManager : MonoBehaviour
{
    public bool firstTime = true;

    public int generatedSeededNumber;

    public List<GameObject> playersList;

    public static OnlineMultiplayerManager instance;
    public List<PlayerDataManager> allPlayers;
    public Dealer Dealer;
    public List<Transform> playerSpawnPoints;
    public PlayerDataManager playerDataManager;
    [SerializeField] GameObject parentToSpawnObject;

    public GameObject radialSliderButton;
    public GameObject foldButton;

    public PlayerBoy playerBoy;

    public List<Card> generatedCommunityCards = new();

    public int currentTurnIndex = 1;

    //Added by Dev
    public PlayerBoy playerToInstantiate;
    GameObject instantiatedGameObject;

    public PlayerDataManager _localPlayerDataManager;
    public bool gameStarted = false;
    public bool inGame = false;
    public bool imDealer = false;

    public int roundNumber = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        OnStartGame();
    }

    public void PopulatePlayers()
    {
        Debug.Log("Populating Players");
       
    }

    public void AddPlayer(PlayerDataManager pdm)
    {
        allPlayers.Add(pdm);
        Dealer.players.Add(pdm.gameObject.GetComponent<PlayerBoy>());
    }

    public void OnStartGame()
    {
        StartCoroutine(StartGameDelayed());
    }

    IEnumerator StartGameDelayed()
    {
        if (roundNumber == 1)
        {
            currentTurnIndex = 2;
        }
        PlayerInstantiate();
        
        //UI_MatchIsStartingPopup matchPopup = (UI_MatchIsStartingPopup)UI_Manager.Instance.OpenIndepenedantPanel(typeof(UI_MatchIsStartingPopup));
        yield return new WaitForSeconds(1f);
        SettingPositions();

        //OnPlayerJoinGame();
        inGame = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;

        yield return new WaitForSeconds(1f);

        //if (matchPopup != null)
        //    Destroy(matchPopup.gameObject);

        //AssignTurnToPlayer1Temp();

        //Commented By Dev
        //gameStarted = true;

        if (PhotonNetwork.LocalPlayer.UserId.Equals(PhotonNetwork.PlayerList[0].UserId))
        {
            imDealer = true;
            _localPlayerDataManager.GetComponent<PhotonView>().RPC("DealerTrue", RpcTarget.All);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                _localPlayerDataManager.photonView.RPC("PlayGameStarter", RpcTarget.All);
            }
            if (_localPlayerDataManager.GetComponent<PlayerBoy>().dealerBool)
            {
                _localPlayerDataManager.photonView.RPC("DealerTagEnabler", RpcTarget.All);
            }
            _localPlayerDataManager.photonView.RPC("TurnBoolTester", RpcTarget.All);
            //_localPlayerDataManager.GetComponent<PlayerBoy>().photonView.RPC("PlayerRankSetterRPC", RpcTarget.All);
        }

        //_localPlayerDataManager.CommunityCardsReplace();

        //UI_Manager.Instance.OpenPanel(typeof(UI_GamePlay), true);

        _localPlayerDataManager.BettingValuesSetter(ref playerBoy.lastBetLocalPlayer, ref playerBoy.playerCurrentTotalBet);
    }

    public void PlayerInstantiate()
    {
        instantiatedGameObject = PhotonNetwork.Instantiate(playerDataManager.gameObject.name, playerSpawnPoints[0].position, playerSpawnPoints[0].rotation);
        _localPlayerDataManager = instantiatedGameObject.GetComponent<PlayerDataManager>();
        playerBoy = _localPlayerDataManager.GetComponent<PlayerBoy>();
        instantiatedGameObject.GetComponent<RectTransform>().SetParent(parentToSpawnObject.transform);
        instantiatedGameObject.GetComponent<RectTransform>().localScale = Vector3.one;
    }


    public void SettingPositions()
    {
        for (int  i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Debug.Log("Testing Position Setter");
            allPlayers[i].gameObject.transform.position = playerSpawnPoints[i].position;
            allPlayers[i].gameObject.transform.rotation = playerSpawnPoints[i].rotation;
            allPlayers[i].gameObject.GetComponent<RectTransform>().SetParent(parentToSpawnObject.transform);
            allPlayers[i].gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    public void EndTurn()
    {
        Debug.Log("end turn");
        _localPlayerDataManager.photonView.RPC("EndTurnRPC", RpcTarget.All);
        TurnChanges();

        _localPlayerDataManager.BettingValuesSetter(ref playerBoy.lastBetLocalPlayer, ref playerBoy.playerCurrentTotalBet);

        _localPlayerDataManager.AddCoinsToPotPlayerDataManager();



    }

    public void TurnChanges()
    {
        Debug.Log(Dealer.instance.currentBet);
        _localPlayerDataManager.photonView.RPC("CallFunction", RpcTarget.All);
        
        Debug.Log(Dealer.instance.currentBet);
    }

    public void FoldButtonClick()
    {
        _localPlayerDataManager.FoldButtonClick();
    }

}
