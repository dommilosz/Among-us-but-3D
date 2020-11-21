using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmongUsLobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject playersCount;
    public bool starting = false;
    public float timer = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersCount();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (starting)
                changeTimer(false);
            else changeTimer(true);
                    
        }
    }

    [PunRPC]
    private void changeTimer(bool v,float t=5f)
    {
        timer = t; starting = v;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("changeTimer", RpcTarget.All,new object[]{v,t});
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            changeTimer(false);
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            changeTimer(false);
    }
    public void UpdatePlayersCount()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            int count = PhotonNetwork.PlayerList.Length;
            var label = playersCount.GetComponent<TextMeshProUGUI>();
            var max = PhotonNetwork.CurrentRoom.MaxPlayers;
            label.text = $"{count}/{max}";

            if (enoughPlayers())
            {
                label.color = Color.green;
                if (PhotonNetwork.IsMasterClient)
                    label.text = $"{count}/{max} (Press Enter)";
            }
            else
            {
                label.color = Color.red;
                label.text = $"{count}/{max} (MIN: {minPlayers})";
            }

            if (starting)
            {
                label.text = $"{count}/{max} (Start in {Math.Round(timer)})";
                timer -= Time.deltaTime;
                if (timer < 0) StartGame();
            }
        }
    }

    public void StartGame()
    {
        if (canStart())
        {
            starting = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            int[] Impostors = new int[(int)SettingsHandler.getSetting("Impostors")];
            foreach (var item in PhotonNetwork.PlayerList)
            {
                item.GetPlayerInfo().setSetting("isImpostor", false);
            }
            for (int i = 0; i < (int)SettingsHandler.getSetting("Impostors"); i++)
            {
                var impo_rnd = Random.Range(0, PhotonNetwork.CountOfPlayers - 1);
                while (Enumerable.Contains(Impostors, impo_rnd)) impo_rnd = Random.Range(0, PhotonNetwork.PlayerList.Length - 1);
                Impostors[i] = impo_rnd;

                PhotonNetwork.PlayerList[impo_rnd].GetPlayerInfo().setSetting("isImpostor", true);
            }
            PhotonNetwork.LoadLevel($"AmongUs_{(string)SettingsHandler.getSetting("Map")}");
        }
    }
    public int minPlayers = 7;
    public bool canStart()
    {
        if (!enoughPlayers()) return false;
        if (!PhotonNetwork.IsMasterClient) return false;
        return true;
    }
    public bool enoughPlayers()
    {
        minPlayers = (3 * (int)SettingsHandler.getSetting("Impostors")) + 1;
        if (PhotonNetwork.PlayerList.Length >= minPlayers) return true;
        return false;
    }
}
