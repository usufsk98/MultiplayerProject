using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class OnlineMultiplayerManager : MonoBehaviour
{

    public List<GameObject> playersList;

    public static OnlineMultiplayerManager instance;
    public List<PlayerDataManager> allPlayers;
    public Dealer Dealer;
    public List<Transform> playerSpawnPoints;
    public PlayerDataManager playerDataManager;
    [SerializeField] GameObject parentToSpawnObject;

    public GameObject radialSliderButton;
    public GameObject foldButton;

    public int playerCurrentTotalBet;

    public int lastBetLocalPlayer;

    public int totalBetOverNetwork;

    public int currentTurnIndex = 1;

    //Added by Dev
    public PlayerBoy playerToInstantiate;
    GameObject instantiatedGameObject;

    public PlayerDataManager _localPlayerDataManager;
    public bool gameStarted = false;
    public bool inGame = false;
    public bool imDealer = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
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

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            imDealer = true;
            _localPlayerDataManager.GetComponent<PhotonView>().RPC("SendingAboutDealer", RpcTarget.All);
            _localPlayerDataManager.GetComponent<PhotonView>().RPC("DealerTrue", RpcTarget.All);
            _localPlayerDataManager.photonView.RPC("PlayGameStarter", RpcTarget.All);
            if (_localPlayerDataManager.GetComponent<PlayerBoy>().dealerBool)
            {
                _localPlayerDataManager.GetComponent<PhotonView>().RPC("DealerTagEnabler", RpcTarget.All);
            }
        }
        _localPlayerDataManager.GetComponent<PhotonView>().RPC("BigAndSmallBlindSetter", RpcTarget.All);
        _localPlayerDataManager.photonView.RPC("TurnBoolTester", RpcTarget.All);

        //UI_Manager.Instance.OpenPanel(typeof(UI_GamePlay), true);
    }
    public void OnPlayerJoinGame()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            instantiatedGameObject = PhotonNetwork.Instantiate(playerDataManager.gameObject.name, playerSpawnPoints[i].transform.position, playerSpawnPoints[i].transform.rotation);
            
            Debug.Log("Player " + i +"Joined the Game");
        }
        
    }

    public void PlayerInstantiate()
    {
        instantiatedGameObject = PhotonNetwork.Instantiate(playerDataManager.gameObject.name, playerSpawnPoints[0].position, playerSpawnPoints[0].rotation);
        _localPlayerDataManager = instantiatedGameObject.GetComponent<PlayerDataManager>();
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
        Debug.Log(Dealer.instance.currentBet);
        _localPlayerDataManager.photonView.RPC("EndTurnRPC", RpcTarget.All);
        _localPlayerDataManager.photonView.RPC("TurnBoolTester", RpcTarget.All);
        _localPlayerDataManager.photonView.RPC("CallFunction", RpcTarget.All);

        lastBetLocalPlayer = Dealer.instance.currentBet;
        playerCurrentTotalBet += Dealer.instance.currentBet;
    }

}
