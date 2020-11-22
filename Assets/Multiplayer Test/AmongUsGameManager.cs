using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmongUsGameManager : MonoBehaviourPunCallbacks
{
    public GameObject playersPlaceHolder;
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        var player = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation, 0);
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

        foreach (var item in players)
        {
            item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName;
            item.GetComponent<PlayerInfo>().setSetting("PlayerName", item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName);
            item.transform.parent = playersPlaceHolder.transform;
            if (!PlayerInfo.isMine(item))
            {
                if(item.transform.Find("PlayerCamera")!=null&& item.transform.Find("Point Light") != null)
                {
                    Destroy(item.transform.Find("PlayerCamera").gameObject);
                    Destroy(item.transform.Find("Point Light").gameObject);

                    item.GetComponent<PlayerActions>().enabled = false;
                }
            }
            else
            {
                item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName+ " (ME)";
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
        
        List<string> claimed_colors = new List<string>();
        foreach (var item in PhotonNetwork.PlayerList)
        {
            var obj = GameObject.Find("Color_" + (string)item.GetPlayerInfo().getSetting("Color"));
            obj.transform.Find("off").gameObject.SetActive(true);
            claimed_colors.Add((string)item.GetPlayerInfo().getSetting("Color"));
        }
        foreach (var item in Enums.Colors.AllColors)
        {
            if (!claimed_colors.Contains(item))
            {
                newPlayer.GetPlayerInfo().setSetting("Color", item);
            }
        } 
    }
}
