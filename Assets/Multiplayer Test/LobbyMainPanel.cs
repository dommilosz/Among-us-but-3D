using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class LobbyMainPanel : MonoBehaviourPunCallbacks
{
    [Header("Connecting Panel")]
    public GameObject ConnectingPanel;

    [Header("Login Panel")]
    public GameObject LoginPanel;

    public InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public InputField RoomNameInputField;
    public InputField MaxPlayersInputField;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;

    public Button StartGameButton;
    public GameObject PlayerListEntryPrefab;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    public string levelToLoad = "";
    public string lobbyScene = "lobby";

    public static bool debug = false;

    #region UNITY

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        PlayerNameInput.text = "Player " + Random.Range(1000, 10000);

        SaveState.InitState();
        var savedname = SaveState.PlayerPreferences.Get("Username", "");

        if (savedname != "")
        {
            ConnectToMasterWithUsername(savedname);
        }
        else if (debug)
        {
            ConnectToMasterWithUsername(PlayerNameInput.text);
        }
        else
        {
            this.SetActivePanel(LoginPanel.name);
        }
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(SelectionPanel.name);
        if (debug)
        {
            RoomOptions options = new RoomOptions { MaxPlayers = 10, PlayerTtl = 0, BroadcastPropsChangeToAll = true, PublishUserId = true };
            PhotonNetwork.CreateRoom("debug", options, null);
        }
        if (RPCHandler.IsGameJoinPending)
        {
            PhotonNetwork.JoinRoom(RPCHandler.GetGameToJoin());
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();

        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PopupScript.SinglePopup(message, "OK", PopupScript.ButtonType.Info);

        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopupScript.SinglePopup(message, "OK", PopupScript.ButtonType.Info);

        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);

        PopupScript.SinglePopup(message, "OK", PopupScript.ButtonType.Info);

        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(lobbyScene);
        return;
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;
    }
    #endregion

    #region UI CALLBACKS

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        SetActivePanel(SelectionPanel.name);
    }

    public async void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;
        bool nameValid = !roomName.Equals(string.Empty);
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;
        
        byte maxPlayers;
        bool countValid = byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
        if(maxPlayers<4 || maxPlayers > 20)
        {
            countValid = false;
        }

        if (!nameValid || !countValid)
        {
            if(!await PopupScript.YNPopup("Some properties are invalid. Do you want to continue with fixed ones?", "Yes", "No"))
            {
                return;
            }
        }
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 4, 20);
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 0, BroadcastPropsChangeToAll = true, PublishUserId = true };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(JoinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;
        ConnectToMasterWithUsername(playerName);
    }

    public void ConnectToMasterWithUsername(string username)
    {
        if (username.Length < 20 && username.Length > 3)
        {
            PhotonNetwork.LocalPlayer.NickName = username;
            PhotonNetwork.ConnectUsingSettings();
            PlayerNameInput.text = username;
            SaveState.PlayerPreferences["Username"] = username;
            SaveState.WriteState();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    public void OnLogOutButtonClicked()
    {
        PhotonNetwork.Disconnect();
        SetActivePanel("LoginPanel");
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(RoomListPanel.name);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel(levelToLoad);
    }

    #endregion

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    private void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
        InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        ConnectingPanel.SetActive(activePanel.Equals(ConnectingPanel.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }
}
