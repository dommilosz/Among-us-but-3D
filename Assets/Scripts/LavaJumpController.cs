using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LavaJumpController : MonoBehaviour
{
    public Player playerToDrop;
    public bool skipped = false;
    public bool tie = false;
    public GameObject text;
    public GameObject playerbestmodel;

    public TimedCallback MsgShow;
    public TimedCallback MsgHide;

    // Start is called before the first frame update
    void Start()
    {
        MsgShow = new TimedCallback(ShowMsg, 5);
        MsgHide = new TimedCallback(HideMsg, 5);
        MsgShow.Start();
        if (skipped || tie)
        {
            playerbestmodel.SetActive(false);
            return;
        }
        else
        {
            var colorToDrop = (string)playerToDrop.GetPlayerInfo().getSetting("Color");
            playerbestmodel.transform.Find("Image").GetComponent<Image>().color = Enums.Colors.getColorObjByName(colorToDrop).color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MsgShow.Tick();
        MsgHide.Tick();
    }

    public string GetEjectMsg()
    {
        try
        {
            string NoOneBase = "No one was ejected: ";
            string EjectedBase = "%p was ejected";
            string KnownEjectedBase = "%p was %n The Impostor";
            if (skipped) return NoOneBase + "Skipped";
            if (tie) return NoOneBase + "Tie";

            string RawMsg;
            if ((bool)SettingsHandler.getSetting("Confirm_Ejects"))
            {
                RawMsg = KnownEjectedBase;
            }
            else
            {
                RawMsg = EjectedBase;
            }
            bool isImpostor = (bool)playerToDrop.GetPlayerInfo().getSetting("isImpostor");
            return RawMsg.Replace("%p", playerToDrop.NickName).Replace("%n ", isImpostor ? "" : "Not ");
        }
        catch
        {
            return "Player ejected on his own.";
        }
    }

    public void ShowMsg()
    {
        text.GetComponent<TMPro.TextMeshProUGUI>().text = GetEjectMsg();
        text.SetActive(true);

        if (!tie && !skipped)
        {
            try
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    playerToDrop.GetPlayerInfo().setSetting("Alive", false);
                }
            }
            catch { }
        }
        MsgHide.Start();
    }

    public void HideMsg()
    {
        PhotonNetwork.LocalPlayer.GetPlayerObject().transform.position = GameObject.Find("AfterMeetingLocation").transform.position;
        GameObject.Find("MeetingCanvas").Destroy();
        TimedAbility.ResetAllAbilities();
    }
}
