using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingRenderer : MonoBehaviour
{
    public GameObject MeetingPlayer;
    public MeetingHandler meetingHandler;
    public static bool shouldRedraw = false;
    public int framesToRender = 20;
    // Start is called before the first frame update
    void Start()
    {
        meetingHandler.meetingRenderer = this;
        Render();
    }

    // Update is called once per frame
    void Update()
    {
        framesToRender -= 1;
        if (framesToRender <= 0)
        {
            Render();
            framesToRender = 20;
        }
        
    }

    public void Render()
    {
        shouldRedraw = false;
        foreach (var item in GameObject.FindGameObjectsWithTag("Meeting_Player"))
        {
            item.Destroy();
        }
        int x = 10 - 300;
        int y = -10 + 115;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if ((bool)item.GetPlayerInfo().getSetting("Alive"))
            {
                renderPlayer(item,x,y);
                x += 140;
                if (x >= 430) { y -= 50; x = 10; }
            }
        }
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (!(bool)item.GetPlayerInfo().getSetting("Alive"))
            {
                renderPlayer(item,x,y);
                x += 140;
                if (x >= 430) { y -= 50; x = 10; }
            }
        }
    }

    public void renderPlayer(Player item,int x,int y)
    {
        var mP = Instantiate(MeetingPlayer);
        mP.transform.SetParent(transform);
        mP.transform.localPosition = new Vector3(x, y);
        mP.transform.localPosition = new Vector3(x, y);
        mP.name = $"Meeting_{item.NickName}";
        mP.tag = "Meeting_Player";
        mP.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = item.NickName;

        if( (bool) PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("isImpostor")){
            if ((bool)item.GetPlayerInfo().getSetting("isImpostor"))
                mP.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1f,0f,0f);
        }

        if (meetingHandler.ReportingPlayerName == item.NickName)
        {
            mP.transform.Find("Report").gameObject.SetActive(true);
        }
        if (item.CustomProperties["Voted"] != null && (string)item.CustomProperties["Voted"] != "")
        {
            mP.transform.Find("Voted").gameObject.SetActive(true);
        }
        mP.GetComponent<Image>().color = Enums.Colors.getColorByName((string)item.GetPlayerInfo().getSetting("Color"));
        if (!(bool)item.GetPlayerInfo().getSetting("Alive"))
        {
            mP.transform.Find("Dead").gameObject.SetActive(true);
        }
        if (PhotonNetwork.LocalPlayer.CustomProperties["Voted"]!=null&& (string)PhotonNetwork.LocalPlayer.CustomProperties["Voted"]!=""&& (string)PhotonNetwork.LocalPlayer.CustomProperties["Voted"]==item.NickName)
        {
            mP.GetComponent<Image>().color = new Color(1, 1, 0, 1);
        }
        if (!(bool)item.GetPlayerInfo().getSetting("Alive"))
        {
            mP.transform.Find("VoteBtn").gameObject.SetActive(false);
        }
        if(meetingHandler.Phase== "Proceeding")
        {
            mP.transform.Find("PlayersVoted").GetComponentInChildren<VoteSpawner>().TargetVote = item.NickName;
            mP.transform.Find("PlayersVoted").gameObject.SetActive(true);
        }
    }
}
