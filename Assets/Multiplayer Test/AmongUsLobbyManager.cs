﻿using Photon.Pun;
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
    public GameObject roleCanvas;
    public bool starting = false;
    public float timer = 5;
    public float role_timer = 5;
    public bool showingRole = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersCount();

        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.Return))
        {
            if (starting)
                changeTimer(false);
            else if(enoughPlayers()) changeTimer(true);
        }

        if (showingRole)
        {
            role_timer -= Time.deltaTime;
            if (role_timer < 0) LoadGameScene();
        }
    }

    private void LoadGameScene()
    {
        showingRole = false;
        PhotonNetwork.LoadLevel($"AmongUs_{(string)SettingsHandler.getSetting("Map")}");
    }

    [PunRPC]
    private void changeTimer(bool v, float t = 5f)
    {
        timer = t; starting = v;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("changeTimer", RpcTarget.Others, new object[] { v, t });
        }
    }

    [PunRPC]
    private void showRole()
    {
        showingRole = true;
        if (GameObject.Find("Role_Canvas") == null)
        {
            GameObject.Instantiate(roleCanvas).name = "Role_Canvas";
        }
        var canvas = GameObject.Find("Role_Canvas");
        bool isImpostor = (bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("isImpostor");
        int impCount = (int)SettingsHandler.getSetting("Impostors");
        int oppCount = isImpostor? PhotonNetwork.PlayerList.Length-impCount : (int)SettingsHandler.getSetting("Impostors");
        string oppRole = isImpostor ? "Crewmates" : (impCount > 1 ? "Impostors" : "Impostor");

        canvas.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"There is {oppCount} <b><color={(isImpostor?"green":"red")}>{oppRole}</color></b> Among Us";

        canvas.transform.Find("Crewmate").gameObject.SetActive(!isImpostor);
        canvas.transform.Find("Impostor").gameObject.SetActive(isImpostor);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("showRole", RpcTarget.Others);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.IsMasterClient)
            changeTimer(false);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
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
            showRole();
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
