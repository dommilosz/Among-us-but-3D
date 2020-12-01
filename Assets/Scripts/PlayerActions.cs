using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject overlayCanvas;
    public GameObject mapCanvas;
    GameObject canvas = null;

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
        if ((bool)playerInfo.getSetting("isImpostor"))
        {
            sabotageButton.SetActive(true);
            useButton.SetActive(false);
            killButton.SetActive(true);

            int killCD = (int)Math.Round(PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillCd);
            if (PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillCd == 0)
            {
                killButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"KILL (Ready)";
            }
            else
            {
                killButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"KILL ({killCD})";
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
            ToggleMap();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KillAction();
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

    public static void ReportAction()
    {

    }

    public static void KillAction()
    {
        PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().KillClosestPlayer();
    }

    public void ToggleMap()
    {
        if (GameObject.Find("MapCanvas(Clone)") != null)
        {
            Destroy(GameObject.Find("MapCanvas(Clone)"));
        }
        else
        {
            Instantiate(mapCanvas);
        }
    }
}
