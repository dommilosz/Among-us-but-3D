using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public GameObject overlayCanvas;
    public GameObject mapCanvas;
    GameObject canvas = null;
    public int ReportDistance = 100;
    public bool canReport = false;
    public GameObject MeetingCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("OverlayCanvas") == null)
        {
            canvas = Instantiate(overlayCanvas);
            canvas.name = "OverlayCanvas";
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("OverlayCanvas") == null)
        {
            Start();
        }
        var canvas = GameObject.Find("OverlayCanvas");
        //0: Use
        //1: Kill
        //2: Report
        //3: Sabotage
        //4: Vent

        var useButton = canvas.transform.GetChild(0).gameObject;
        var killButton = canvas.transform.GetChild(1).gameObject;
        var reportButton = canvas.transform.GetChild(2).gameObject;
        var sabotageButton = canvas.transform.GetChild(3).gameObject;
        var ventButton = canvas.transform.GetChild(4).gameObject;

        useButton.SetActive(false);
        killButton.SetActive(false);
        sabotageButton.SetActive(false);

        var playerInfo = PlayerInfo.getPlayerInfo();
        var player = PlayerInfo.getPlayer();
        bool alive = (bool)playerInfo.getSetting("Alive");

        RaycastHit hit;

        // if raycast hits, it checks if it hit an object with the tag Player
        if (Physics.Raycast(transform.Find("Orientation").position, transform.Find("Orientation").forward, out hit, ReportDistance) && hit.collider.gameObject.CompareTag("Body"))
        {
            reportButton.transform.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            canReport = true;
        }
        else
        {
            reportButton.transform.Find("Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        }

        killButton.transform.Find("Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

        var killScript = PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>();
        if (killScript.SelectedPlayer.Length > 0)
        {
            killButton.transform.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        }
        useButton.transform.Find("Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

        if (PlayerInfo.getPlayerInfo().canUse)
        {
            useButton.transform.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        }

        if ((bool)playerInfo.getSetting("isImpostor"))
        {
            sabotageButton.SetActive(true);
            useButton.SetActive(false);
            killButton.SetActive(true);

            int killCD = (int)Math.Round(PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillCd);
            if (PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillCd == 0)
            {
                killButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"";
            }
            else
            {
                killButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{killCD}";
            }
            
        }
        else
        {
            sabotageButton.SetActive(false);
            useButton.gameObject.SetActive(true);
            killButton.gameObject.SetActive(false);
        }
        if (playerInfo.VentStanding != null || (bool)playerInfo.getSetting("inVent"))
        {
            sabotageButton.SetActive(false);
            ventButton.SetActive(true);
        }
        else
        {
            ventButton.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObject.Find("MeetingCanvas") != null) return;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ToggleSabotage();
            }
            else
            {
                ToggleMap();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Q)&&alive)
        {
            if (GameObject.Find("MeetingCanvas") != null) return;
            KillAction();
        }
        if (Input.GetKeyDown(KeyCode.R)&&alive)
        {
            if (GameObject.Find("MeetingCanvas") != null) return;
            ReportAction(hit.transform.name.Replace("Body ",""));
        }

        if (gameObject.transform.position.y < -1)
        {
            gameObject.transform.position = GameObject.Find("Game Manager").transform.position;
        }
    }

    public static void UseAction()
    {

    }

    public static void SabotageAction()
    {

    }

    public static void VentAction()
    {
        var playerInfo = PlayerInfo.getPlayerInfo();
        if (!(bool)playerInfo.getSetting("inVent"))
        {
            VentScript.enterVentS(playerInfo.VentStanding.gameObject);
            return;
        }
        if ((bool)playerInfo.getSetting("inVent"))
        {
            VentScript.exitVentS(playerInfo.VentStanding.gameObject);
            return;
        }
    }

    public static void ReportAction(string color)
    {
        if (GameObject.Find("MeetingCanvas") != null) return;
        var playerInfo = PlayerInfo.getPlayerInfo();
        var PA = PlayerInfo.getPlayer().GetComponent<PlayerActions>();
        if (color != "meeting")
            if (!PA.canReport) return;

        PhotonView photonView = PhotonView.Get(PA);

        photonView.RPC("BodyReported", RpcTarget.All, new object[] { PhotonNetwork.LocalPlayer.NickName, color });

    }

    [PunRPC]
    public void BodyReported(string reportingPlayer,string color)
    {

        try
        {
            if (color != "meeting")
            {
                Enums.Colors.getColorByName(color);
            }
            if (color == "meeting")
            {
                color = "red";
            }
            if (GameObject.Find("MeetingCanvas") != null) return;
            var PA = PlayerInfo.getPlayer().GetComponent<PlayerActions>();
            var PAc = Instantiate(PA.MeetingCanvas);
            PAc.GetComponent<DBReported>().bodyColor = color;
            PAc.name = "MeetingCanvas";
            PAc.transform.Find("MeetingCanvas").GetComponent<MeetingHandler>().ReportingPlayerName = reportingPlayer;
        }
        catch { }
    }

    [PunRPC]
    public void PlayerVoted()
    {
        MeetingRenderer.shouldRedraw = true;
    }

    public static void KillAction()
    {
        PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillClosestPlayer();
    }

    public void ToggleMap()
    {
        if (GameObject.Find("MapCanvas") != null)
        {
            Destroy(GameObject.Find("MapCanvas"));
        }
        else
        {
            Instantiate(mapCanvas).name = "MapCanvas";
        }
    }

    public void ToggleSabotage()
    {
        if (GameObject.Find("MapCanvas") != null)
        {
            Destroy(GameObject.Find("MapCanvas"));
            MouseUnLocker.LockMouse();
        }
        else
        {
            var canvas = Instantiate(mapCanvas);
            canvas.name = "MapCanvas";
            canvas.GetComponent<MapScript>().sabotage = true;
            MouseUnLocker.UnlockMouse();
        }
    }
}
