using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmongUsGameManager : MonoBehaviourPunCallbacks
{
    GameObject playersPlaceHolder;
    public GameObject playerPrefab;
    public bool debug = false;
    public Dictionary<string, object> TempData = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        playersPlaceHolder = GameObject.Find("Players");
        TempData = new Dictionary<string, object>();
        TempData.Clear();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            SaveTempData();
        }
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;

        TimedAbility.ResetAllAbilities();

        PhotonNetwork.LocalPlayer.GetPlayerInfo().Start();
        PhotonNetwork.LocalPlayer.GetPlayerObject().transform.position = GetGameManager().transform.position;
        PhotonNetwork.LocalPlayer.GetPlayerObject().transform.rotation = GetGameManager().transform.rotation;

        PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillAbility.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        playersPlaceHolder = GameObject.Find("Players");
        if (PhotonNetwork.CurrentRoom == null)
        {
            LobbyMainPanel.debug = debug;
            AmongUsLobbyManager.debug = debug;
            SceneManager.LoadScene("AmongUS3D-LobbyScene");
            return;
        }

        var players = GameObject.FindGameObjectsWithTag("Player");
        DontDestroyOnLoad(GameObject.Find("Players"));
        foreach (var item in players)
        {
            item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName;
            item.GetComponent<PlayerInfo>().setSetting("PlayerName", item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName);
            item.transform.SetParent(playersPlaceHolder.transform);
            if (!PlayerInfo.isMine(item))
            {
                if ((bool)item.GetComponent<PlayerInfo>().getSetting("Invisible"))
                {
                    ShowPlayer(false, item);
                }
                else
                {
                    ShowPlayer(true, item);
                }
                if (!(bool)item.GetComponent<PlayerInfo>().getSetting("Alive")&& !(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Alive"))
                {
                    ShowPlayer(true,item);
                }
                if (item.transform.Find("PlayerCamera") != null && item.transform.Find("Point Light") != null)
                {
                    Destroy(item.transform.Find("PlayerCamera").gameObject);
                    Destroy(item.transform.Find("Point Light").gameObject);

                    item.GetComponent<PlayerActions>().enabled = false;
                    item.GetComponent<KillScript>().enabled = false;
                }
            }
            else
            {
                item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName + " (ME)";
                item.transform.Find("Orientation").Find("playerbestmodel").gameObject.SetActive(false);
            }
        }

        LoadTempData();
    }

    public void ShowPlayer(bool show,GameObject item)
    {
        item.transform.Find("Orientation").Find("playerbestmodel").gameObject.SetActive(show);
        item.transform.Find($"{item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName}- [label]").gameObject.SetActive(show);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Destroy(PlayerInfo.getPlayerObject(otherPlayer));
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public void SpawnPlayer(Player player)
    {
        //if (!PhotonNetwork.IsMasterClient) return;
        if (GameObject.Find("Player " + PhotonNetwork.LocalPlayer.NickName) == null)
        {
            SpawnPlayerF(player);
        }
    }
    public void SpawnPlayerF(Player player)
    {
        var playerObj = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation, 0);
        playerObj.name = "Player " + player.NickName;
    }

    public static AmongUsGameManager GetGameManager()
    {
        return GameObject.Find("Game Manager").GetComponent<AmongUsGameManager>();
    }

    public void SaveTempData()
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Temp", TempData);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public void LoadTempData()
    {
        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        TempData = (Dictionary<string, object>)ht["Temp"];
        if (TempData == null)
        {
            TempData = new Dictionary<string, object>();
        }
    }
}
