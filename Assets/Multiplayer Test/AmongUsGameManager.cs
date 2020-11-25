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
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        playersPlaceHolder = GameObject.Find("Players");
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        PhotonNetwork.LocalPlayer.GetPlayerObject().transform.position = GetGameManager().transform.position;
        PhotonNetwork.LocalPlayer.GetPlayerObject().transform.rotation = GetGameManager().transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("AmongUS3D-LobbyScene");
            return;
        }

        var players = GameObject.FindGameObjectsWithTag("Player");
        DontDestroyOnLoad(GameObject.Find("Players"));
        foreach (var item in players)
        {
            item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName;
            item.GetComponent<PlayerInfo>().setSetting("PlayerName", item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName);
            item.transform.parent = playersPlaceHolder.transform;
            if (!PlayerInfo.isMine(item))
            {
                if (item.transform.Find("PlayerCamera") != null && item.transform.Find("Point Light") != null)
                {
                    Destroy(item.transform.Find("PlayerCamera").gameObject);
                    Destroy(item.transform.Find("Point Light").gameObject);

                    item.GetComponent<PlayerActions>().enabled = false;
                }
            }
            else
            {
                item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName + " (ME)";
                item.transform.Find("Orientation").Find("playerbestmodel").gameObject.active = false;
            }
        }
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
}
