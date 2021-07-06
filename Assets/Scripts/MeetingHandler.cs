using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MeetingHandler : MonoBehaviour
{
    public bool voted = false;
    public string ReportingPlayerName = "";
    public MeetingRenderer meetingRenderer;
    public GameObject lava;
    public GameObject skip;
    public TimedCallbackSequence sequence;

    public static string SkipSpecialValue = "Special::Skip";

    // Start is called before the first frame update
    void Start()
    {
        voted = false;
        int _DiscusionTime = (int)SettingsHandler.getSetting("DiscusionTime");
        int _VotingTime = (int)SettingsHandler.getSetting("VotingTime");
        int _ProceedingTime = 5;

        sequence = new TimedCallbackSequence();
        sequence.AddElement(DiscussionTime, _DiscusionTime, "Discussion");
        sequence.AddElement(VotingTime, _VotingTime, "Voting");
        sequence.AddElement(ProcedingTime, _ProceedingTime, "Proceeding");
        transform.Find("Discussion").gameObject.SetActive(true);
        sequence.Start();
        VoteClass.Reset();
        Hashtable ht = new Hashtable();
        ht.Add("Voted", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    // Update is called once per frame
    void Update()
    {
        sequence.Tick();
        transform.Find("TimeLeft").GetComponent<TMPro.TextMeshProUGUI>().text = $"{sequence.GetCurrentEvent().name} {sequence.GetTimeLeftOfCurrentEvent()}s";

        allVoted = true;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            bool voted = false;
            if (!(bool)item.GetPlayerInfo().IsAlive()) voted = true;
            if ((string)item.CustomProperties["Voted"] != null && (string)item.CustomProperties["Voted"] != "") voted = true;

            if (!voted) { allVoted = false; break; }
        }
        if (allVoted)
        {
            sequence.GoTo("Proceeding");
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            AmongUsGameManager.GetGameManager().TempData["MeetingTimer"] = sequence.GetCurrentEvent().RemDelay;
            AmongUsGameManager.GetGameManager().TempData["MeetingSeqIndex"] = sequence.index;
            AmongUsGameManager.GetGameManager().SaveTempData();
        }

        float serverDelay = (float)AmongUsGameManager.GetGameManager().TempData.Get("MeetingTimer", sequence.GetCurrentEvent().RemDelay);
        float clientDelay = sequence.GetCurrentEvent().RemDelay;
        int serverIndex = (int)AmongUsGameManager.GetGameManager().TempData.Get("MeetingSeqIndex", sequence.index);
        int clientIndex = sequence.index;

        if (serverIndex != -1)
        {
            if (serverIndex == clientIndex)
            {
                sequence.GetCurrentEvent().RemDelay = serverDelay;
            }
            if (serverIndex > clientIndex)
            {
                sequence.GetCurrentEvent().RemDelay = 0;
            }
            if (serverIndex < clientIndex)
            {
                sequence.GetCurrentEvent().RemDelay = 10;
            }
        }

    }

    public void DiscussionTime()
    {
        transform.Find("Discussion").gameObject.SetActive(false);
    }
    public void VotingTime()
    {
        skip.SetActive(true);
        skip.GetComponent<VoteSpawner>().TargetVote = SkipSpecialValue;
    }
    public void ProcedingTime()
    {
        CountVotes();
    }

    private void CountVotes()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if ((bool)item.GetPlayerInfo().getSetting("Alive") && (string)item.CustomProperties["Voted"] != null && (string)item.CustomProperties["Voted"] != "")
            {
                string vote = (string)item.CustomProperties["Voted"];
                if (vote == SkipSpecialValue)
                    VoteClass.CountSpecial(vote);
                VoteClass.CountVote(vote);
            }
        }

        var result = VoteClass.GetResult();

        var ljc = lava.GetComponent<LavaJumpController>();

        ljc.skipped = result == VoteResult.Skip;
        ljc.tie = result == VoteResult.Tie;
        ljc.playerToDrop = PlayerInfo.getPlayerByName(result.type);

        lava.SetActive(true);
        gameObject.SetActive(false);
    }

    public bool allVoted = false;
    public void Vote(string player)
    {
        if (sequence.GetCurrentEvent().name != "Voting") return;
        if (!(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Alive")) return;
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
        Vote(SkipSpecialValue);
        transform.Find("Skip").GetComponent<Image>().color = new Color(1, 1, 0, 1);
    }

    public class VoteClass
    {
        public static List<VoteClass> votes = new List<VoteClass>();
        public string player = "";
        public bool special = false;
        public int count = 0;
        public VoteClass(string player, bool special = false)
        {
            this.player = player;
            this.special = special;
            votes.Add(this);
        }

        public static void CountVote(string player)
        {
            foreach (var item in votes)
            {
                if (item.player == player)
                {
                    if (item.special) return;
                    item.count++;
                    return;
                }
            }
            var vote = new VoteClass(player);
            vote.count++;
        }

        public static void CountSpecial(string type)
        {
            foreach (var item in votes)
            {
                if (item.player == type)
                {
                    if (!item.special) return;
                    item.count++;
                    return;
                }
            }
        }

        public static void Reset()
        {
            votes.Clear();
            new VoteClass(SkipSpecialValue, true);
            new VoteClass("Special::Empty", true);
        }

        public static VoteClass GetSpecial(string type)
        {
            foreach (var item in votes)
            {
                if (item.player == type)
                {
                    if (item.special) return item;
                }
            }
            return null;
        }

        public static VoteResult GetResult()
        {
            VoteClass res = null;
            VoteClass res2 = null;
            foreach (var item in votes)
            {
                if (res == null)
                {
                    res2 = res;
                    res = item;
                }

                if (res.count < item.count)
                {
                    res2 = res;
                    res = item;
                }
            }

            if (res2 != null && res.count == res2.count)
            {
                return VoteResult.Tie;
            }

            if (res == GetSpecial(SkipSpecialValue))
            {
                return VoteResult.Skip;
            }
            return new VoteResult(res.player, false);
        }


    }

    public class VoteResult
    {
        public static VoteResult Skip = new VoteResult("skip", true);
        public static VoteResult Tie = new VoteResult("tie", true);
        public string type;
        public bool special;

        public VoteResult(string v1, bool v2)
        {
            this.type = v1;
            this.special = v2;
        }
    }
}
