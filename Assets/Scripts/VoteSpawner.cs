using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteSpawner : MonoBehaviour
{
    public string TargetVote;
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        bool AW = (bool)SettingsHandler.getSetting("Anonymous_Votes");
        int x = -50;
        int y = 0;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if ((bool)item.GetPlayerInfo().getSetting("Alive") && (string)item.CustomProperties["Voted"] != null && (string)item.CustomProperties["Voted"] != "")
            {
                string vote = (string)item.CustomProperties["Voted"];
                if (TargetVote == vote)
                {
                    var pp = Instantiate(playerPrefab);
                    pp.transform.SetParent(transform);
                    pp.transform.localPosition = new Vector3(x, y, 0);
                    x += 20;
                    pp.GetComponent<Image>().color = AW ? Enums.Colors.getColorByName((string)item.GetPlayerInfo().getSetting("Color")) : Enums.Colors.getColorByName("gray");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
