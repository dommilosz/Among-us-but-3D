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

        canvas.transform.GetChild(0).gameObject.SetActive(false);
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        canvas.transform.GetChild(3).gameObject.SetActive(false);

        var playerInfo = PlayerInfo.getPlayerInfo();
        var player = PlayerInfo.getPlayer();
        if ((bool)playerInfo.getSetting("isImpostor"))
        {
            canvas.transform.GetChild(3).gameObject.SetActive(true);
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            canvas.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            canvas.transform.GetChild(3).gameObject.SetActive(false);
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            canvas.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (playerInfo.VentStanding!=null|| (bool)playerInfo.getSetting("inVent"))
        {
            canvas.transform.GetChild(3).gameObject.SetActive(false);
            canvas.transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            canvas.transform.GetChild(4).gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMap();
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

    }

    public void ToggleMap()
    {
        if (GameObject.Find("MapCanvas(Clone)") !=null)
        {
            Destroy(GameObject.Find("MapCanvas(Clone)"));
        }
        else
        {
            Instantiate(mapCanvas);
        }
    }
}
