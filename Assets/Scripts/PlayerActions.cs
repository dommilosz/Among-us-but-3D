using Photon.Pun;
using System;
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
            sabotageButton.SetActive(!PlayerInfo.getPlayerInfo().canUse);
            useButton.SetActive(PlayerInfo.getPlayerInfo().canUse);
            killButton.SetActive(true);

            int killCD = (int)Math.Round(PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillAbility.RemCooldown);
            if (PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillAbility.Ready)
            {
                killButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"";
            }
            else
            {
                killButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{killCD}";
            }
            if (Input.GetKeyDown(KeyCode.E)&& !PlayerInfo.getPlayerInfo().canUse)
            {
                if (GameObject.Find("MeetingCanvas") != null) return;
                ToggleMap();

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
            ToggleMap();

        }
        if (Input.GetKeyDown(KeyCode.Q) && alive)
        {
            if (GameObject.Find("MeetingCanvas") != null) return;
            KillAction();
        }
        if (Input.GetKeyDown(KeyCode.R) && alive)
        {
            if (GameObject.Find("MeetingCanvas") != null || hit.transform == null) return;
            if (!hit.transform.CompareTag("Body")) return;
            ReportAction(hit.transform.name.Replace("Body ", ""));
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
        AmongUsGameManager.GetGameManager().TempData["MeetingSeqIndex"] = -1;
        var playerInfo = PlayerInfo.getPlayerInfo();
        var PA = PlayerInfo.getPlayer().GetComponent<PlayerActions>();
        if (!PA.canReport) return;
        PhotonView photonView = PhotonView.Get(PA);
        photonView.RPC("BodyReported", RpcTarget.All, new object[] { PhotonNetwork.LocalPlayer.NickName, color,false });
    }
    public static void MeetingAction(string color)
    {
        if (GameObject.Find("MeetingCanvas") != null) return;
        AmongUsGameManager.GetGameManager().TempData["MeetingSeqIndex"] = -1;
        var playerInfo = PlayerInfo.getPlayerInfo();
        var PA = PlayerInfo.getPlayer().GetComponent<PlayerActions>();
        PhotonView photonView = PhotonView.Get(PA);
        photonView.RPC("BodyReported", RpcTarget.All, new object[] { PhotonNetwork.LocalPlayer.NickName, color, true });
    }

    public static PhotonView GetPhotonView()
    {
        var PA = PlayerInfo.getPlayer().GetComponent<PlayerActions>();
        return PhotonView.Get(PA);
    }

    public static void Meeting()
    {
        MeetingAction((string)PlayerInfo.getPlayerInfo().getSetting("Color"));
    }

    [PunRPC]
    public void BodyReported(string reportingPlayer, string color,bool meeting)
    {
        try
        {
            BodyScript.DestroyBodies();
            if (GameObject.Find("MeetingCanvas") != null) return;
            var PA = PlayerInfo.getPlayer().GetComponent<PlayerActions>();
            var PAc = Instantiate(PA.MeetingCanvas);
            PAc.GetComponent<DBReported>().meeting = meeting;
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
            MouseUnLocker.LockMouse();
        }
        else
        {
            Instantiate(mapCanvas).name = "MapCanvas";
            MouseUnLocker.UnlockMouse();
        }
    }
}
