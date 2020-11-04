using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject overlayCanvas;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = Instantiate(overlayCanvas);
    }
    // Update is called once per frame
    void Update()
    {
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (playerInfo.isImpostor)
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
        if (playerInfo.VentStanding!=null)
        {
            canvas.transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            canvas.transform.GetChild(4).gameObject.SetActive(false);
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
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        if (!playerInfo.inVent)
        {
            VentScript.enterVentS(playerInfo.VentStanding.gameObject);
            return;
        }
        if (playerInfo.inVent)
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
}
