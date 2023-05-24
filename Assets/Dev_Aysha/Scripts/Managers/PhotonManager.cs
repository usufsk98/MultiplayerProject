using System.Diagnostics;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks, ILobbyCallbacks, IInRoomCallbacks
{
    public int testerInt;
    public static PhotonManager instance;

     [HideInInspector]
    public bool joinedLobbyRoom = false;
    [HideInInspector]
    public bool joiningGameRoom;
    string quickMatch = "QM"; // QuickMatch
    string multiplayerMatch = "MM"; // MultiplayrMatch
    string oneVone = "ovo";
    string twoVtwo = "tvt";
    string threeVthree = "thVth";
    string twoVTwoVTwo = "tvtvt";
    string freeForAll = "ffa";
    string singlePlayer = "sp";
    public int maxPlayersInRoom, teamCount, countOfPlayersPerTeam, playerId = 0;

    Hashtable customPropertyMatchType = new Hashtable();
    Hashtable roomDataHash = new Hashtable();
    public List<RoomInfo> gameRooms = new List<RoomInfo>();


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
 public override void OnEnable()
    {
        joinedLobbyRoom = false;
        //PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        InstantiateMultiplayer();
        base.OnEnable();
    }

    public override void OnDisable()
    {
        //PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        base.OnDisable();
    }
    public void OnDestroy()
    {
        if (this.isActiveAndEnabled)
        {
            Clear();
        }
    }

#region PhotonCallbacks

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        DebugLog.Log("Lobby Joined");
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhoton();
            return;
        }
        else
        {
             PhotonNetwork.LocalPlayer.NickName = PlayerPrefsManager.UserCodeValue;
        }
        base.OnJoinedLobby();
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        testerInt++;
        DebugLog.Log("Player joined the room");
        CheckPlayersToStartGame();
        base.OnJoinedRoom();
    }
    public override void OnLeftRoom()
    {
        DebugLog.Log("Player Left the room");
        base.OnLeftRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        CheckPlayersToStartGame();
        DebugLog.Log("Player entered in room+: "+PhotonNetwork.PlayerList.Length);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        gameRooms = roomList;
        base.OnRoomListUpdate(roomList);
    }
#endregion



#region PhotonFaliureCallbacks

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //joinLobbyRoom();
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //joinLobbyRoom();
        base.OnJoinRoomFailed(returnCode, message);
    }
    #endregion




       #region CustomFunctions

    public void InstantiateMultiplayer()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UnityEngine.Debug.LogError("Poor Internet Connection");
        }
        else
        {
            ConnectToPhoton();
        }
    }
    public void ConnectToPhoton()
    {
        //UnityEngine.Debug.Log("Connect to photon");
        ConnectToServerAgain();
    }
    public void ConnectToServerAgain()
    {
        PhotonNetwork.ConnectUsingSettings();
        // PhotonNetwork.JoinLobby();
    }
   
    public void FreeForAll(int playersCount)
    {
        StartCoroutine(FreeForAllDelayed(playersCount));
    }

    IEnumerator FreeForAllDelayed(int playersCount)
    {

        yield return new WaitForSeconds(0.5f);

        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhoton();
        }
       
        if (gameRooms.Count > 0)
        {
            foreach (var item in gameRooms)
            {
                //if ((string)item.CustomProperties["MatchType"] == null) continue;
                //if ((string)item.CustomProperties["MatchType"] == freeForAll)
                {
                    //if (item.PlayerCount < item.MaxPlayers && item.IsOpen && item.MaxPlayers == playersCount)
                    if (item.PlayerCount < item.MaxPlayers && item.IsOpen)
                    {
                        PhotonNetwork.JoinRoom(item.Name);
                        DebugLog.Log("Join Room");
                        //StartCoroutine(EnableCancelButton());
                        yield break;
                    }
                }
            }
        }
        CreateRoom(playersCount, freeForAll);
    }
   
     public void CreateRoom(int maxPlayers, string matchType)
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhoton();
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Convert.ToByte(maxPlayers);
        roomOptions.PublishUserId = true;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        float roomname = Random.Range(10000, 99999);

        customPropertyMatchType["MatchType"] = matchType;
        roomOptions.CustomRoomProperties = customPropertyMatchType;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "MatchType" };
        DebugLog.Log("Create Room");
        if (!PhotonNetwork.JoinOrCreateRoom(roomname.ToString(), roomOptions, TypedLobby.Default))
        {
            CreateOrJoinRoomInCaseOfError();
        }
    }
    public void JoinGameRoomFreeForAll()
    {
        joiningGameRoom = true;
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = Convert.ToByte(Random.Range(2,7));
        roomOptions.MaxPlayers = 2;
        roomOptions.PublishUserId = true;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        float roomname = Random.Range(10000, 99999);
        bool found = false;
        customPropertyMatchType["MatchType"] = quickMatch;
        roomOptions.CustomRoomProperties = customPropertyMatchType;
        if (gameRooms.Count > 0)
        {
            foreach (var item in gameRooms)
            {
                if (item.CustomProperties[0].Equals(quickMatch))
                {
                    if (item.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
                    {
                        PhotonNetwork.JoinRoom(item.Name);
                        UnityEngine.Debug.Log("Join room");
                        found = true;
                        break;
                    }
                }
            }
        }
        if (!found)
        {
            PhotonNetwork.JoinOrCreateRoom(roomname.ToString(), roomOptions, TypedLobby.Default);
            //UI_Manager.instance.OpenPanel(typeof(UI_Gameplay), true);
        }
    }

    public void CreateOrJoinRoomInCaseOfError()
    {
        FreeForAll(maxPlayersInRoom);
    }
    public void CheckPlayersToStartGame()
    {
        int playersinRoom = ((int)PhotonNetwork.CurrentRoom.PlayerCount);
        if (playersinRoom >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            GameManager.instance.StartGame();
        }
    }

    public void LeaveGameRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            joiningGameRoom = false;
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.LeaveRoom();
        }
    }

    public void Clear()
    {
        joinedLobbyRoom = false;
        joiningGameRoom = false;
        gameRooms.Clear();
    }
#endregion
}
