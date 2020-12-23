using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaJumpController : MonoBehaviour
{
    public Player playerToDrop;
    public bool skipped = false;
    public bool tie = false;
    public float toShowMsg = 5f;
    public float tohideMsg = 5f;
    public GameObject text;
    public GameObject playerbestmodel;
    // Start is called before the first frame update
    void Start()
    {
        if (skipped || tie)
        {
            playerbestmodel.SetActive(false);
            return;
        }
        var colorToDrop = (string)playerToDrop.GetPlayerInfo().getSetting("Color");
        playerbestmodel.transform.Find("Image").GetComponent<Image>().color = Enums.Colors.getColorByName(colorToDrop);
    }

    // Update is called once per frame
    void Update()
    {
        toShowMsg -= Time.deltaTime;
        if (toShowMsg <= 0)
        {
            if (skipped || tie)
            {
                string txt2 =  skipped ? "Skipped" : "Tie" ;
                string txt = $"No one was ejected ({txt2})";
                text.GetComponent<TMPro.TextMeshProUGUI>().text = $"{txt}";
                text.SetActive(true);
            }
            else
            {


                bool isImpostor = (bool)playerToDrop.GetPlayerInfo().getSetting("isImpostor");
                string txt = isImpostor ? "" : "Not ";
                text.GetComponent<TMPro.TextMeshProUGUI>().text = $"{playerToDrop.NickName} was {txt}An Impostor";
                text.SetActive(true);

                if (PhotonNetwork.IsMasterClient)
                {
                    playerToDrop.GetPlayerInfo().setSetting("Alive", false);
                }
            }
            tohideMsg -= Time.deltaTime;
        }
        if (tohideMsg <= 0)
        {
            PhotonNetwork.LocalPlayer.GetPlayerObject().transform.position = GameObject.Find("AfterMeetingLocation").transform.position;
            MouseUnLocker.LockMouse();
            PlayerInfo.getPlayerInfo().MeetingsCooldown = (float)SettingsHandler.getSetting("Emergency_Cooldown");
            PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillCd = (float)SettingsHandler.getSetting("KillCooldown");
            GameObject.Find("MeetingCanvas").Destroy();
        }
    }
}
