using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MeetingHandler : MonoBehaviour
{
    public bool voted = false;
    public float DiscusionTimeLeft = 30f;
    public float VotingTimeLeft = 120f;
    public float ProceedingTimeLeft = 5f;
    public string Phase = "Discussion";
    public float PhaseTime = 30f;
    public string ReportingPlayerName = "";
    public MeetingRenderer meetingRenderer;
    public GameObject lava;
    public GameObject skip;
    // Start is called before the first frame update
    void Start()
    {
        voted = false;
        DiscusionTimeLeft = (int)SettingsHandler.getSetting("DiscusionTime");
        VotingTimeLeft = (int)SettingsHandler.getSetting("VotingTime");
        ProceedingTimeLeft = 5;

        if (VotingTimeLeft <= 0) return;
        Hashtable ht = new Hashtable();
        ht.Add("Voted", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    // Update is called once per frame
    void Update()
    {
        MouseUnLocker.UnlockMouse();
        if (DiscusionTimeLeft <= 0)
        {
            if (VotingTimeLeft > 0) VotingTimeLeft -= Time.deltaTime;
            if (VotingTimeLeft <= 0) VotingTimeLeft = 0;
        }
        if (DiscusionTimeLeft > 0) DiscusionTimeLeft -= Time.deltaTime;
        if (DiscusionTimeLeft <= 0) DiscusionTimeLeft = 0;

        if (VotingTimeLeft <= 0)
        {
            ProceedingTimeLeft -= Time.deltaTime;
        }

        if (ProceedingTimeLeft <= 0)
        {
            List<string> players = new List<string>();
            List<int> votes = new List<int>();
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if ((bool)item.GetPlayerInfo().getSetting("Alive") && (string)item.CustomProperties["Voted"] != null && (string)item.CustomProperties["Voted"] != "")
                {
                    string vote = (string)item.CustomProperties["Voted"];

                    if (!players.Contains(vote)) { players.Add(vote); votes.Add(1); } else
                    {
                        votes[players.IndexOf(vote)] += 1;
                    }
                }
            }
            int max = 0;
            int i = -1;
            for (int i2 = 0; i2 < votes.Count; i2++)
            {
                int item = votes[i2];
                if (item > max)
                {
                    max = item;
                    i = i2;
                }
            }
            var ljc = lava.GetComponent<LavaJumpController>();
            if (votes.Count < 1)
            {
                ljc.skipped = true;
            }
            if (i >= 0)
            {
                votes.RemoveAt(i);
                if (votes.Contains(max))
                {
                    ljc.tie = true;
                }
            }
            if (i < 0)
            {
                ljc.skipped = true;
            }
            else if(players[i]== "%%|Skip|%%")
            {
                ljc.skipped = true;
            }
            else
            {
                ljc.playerToDrop = PlayerInfo.getPlayerByName(players[i]);
            }
            
            lava.SetActive(true);
            gameObject.SetActive(false);
        }

        if (DiscusionTimeLeft > 0)
        {
            transform.Find("Discussion").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Discussion").gameObject.SetActive(false);
        }

        if (DiscusionTimeLeft > 0)
        {
            Phase = "Discussion";
            PhaseTime = DiscusionTimeLeft;
        }else
        if (VotingTimeLeft > 0)
        {
            Phase = "Voting";
            PhaseTime = VotingTimeLeft;
        }
        else if (ProceedingTimeLeft > 0)
        {
            Phase = "Proceeding";
            PhaseTime = ProceedingTimeLeft;

            skip.SetActive(true);
            skip.GetComponent<VoteSpawner>().TargetVote = "%%|Skip|%%";

        }

        transform.Find("TimeLeft").GetComponent<TMPro.TextMeshProUGUI>().text = $"{Phase} {Convert.ToInt32(Math.Round(PhaseTime))}s";

        allVoted = true;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if((bool)item.GetPlayerInfo().getSetting("Alive")&& (string)item.CustomProperties["Voted"] !=null&& (string)item.CustomProperties["Voted"] != "")
            {

            }
            else
            {
                allVoted = false;
            }
        }
        if (allVoted) VotingTimeLeft = 0f;
    }
    public bool allVoted = false;
    public void Vote(string player)
    {
        if (VotingTimeLeft <= 0) return;
        if (!(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Alive")) return;
        if (VotingTimeLeft <= 0) return;
        Hashtable ht = new Hashtable();
        ht.Add("Voted", player);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        voted = true;
        PhotonView photonView = PhotonView.Get(PhotonNetwork.LocalPlayer.GetPlayerObject());
        photonView.RPC("PlayerVoted", RpcTarget.All);

    }

    public void Skip()
    {
        if (!(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Alive")) return;
        Vote("%%|Skip|%%");
        transform.Find("Skip").GetComponent<Image>().color = new Color(1, 1, 0, 1);
    }
}
