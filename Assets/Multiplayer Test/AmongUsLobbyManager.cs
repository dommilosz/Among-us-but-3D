using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AmongUsLobbyManager : MonoBehaviour
{
    public GameObject playersCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersCount();

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            StartGame();
        }
    }

    public void UpdatePlayersCount()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            int count = PhotonNetwork.PlayerList.Length;
            playersCount.GetComponent<TextMeshProUGUI>().text = $"{count.ToString()}/{PhotonNetwork.CurrentRoom.MaxPlayers}";

        }
    }

    public void StartGame()
    {
        if (canStart())
        {
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
                while (Impostors.Contains(impo_rnd)) impo_rnd = Random.Range(0, PhotonNetwork.PlayerList.Length - 1);
                Impostors[i] = impo_rnd;

                PhotonNetwork.PlayerList[impo_rnd].GetPlayerInfo().setSetting("isImpostor", true);
            }
            PhotonNetwork.LoadLevel($"AmongUs_{(string)SettingsHandler.getSetting("Map")}");
        }
    }

    public bool canStart()
    {
        if (!PhotonNetwork.IsMasterClient) return false;
        if (PhotonNetwork.PlayerList.Length >= 4) return true;
        return false;
    }
}
