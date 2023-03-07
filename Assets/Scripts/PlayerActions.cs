using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject overlayCanvas;
    public GameObject mapCanvas;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = Instantiate(overlayCanvas);
    }
    // Update is called once per frame
    void Update()
    {
        //0: Use
        //1: Kill
        //2: Report
        //3: Sabotage
        //4: Vent


        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (playerInfo.isImpostor)
        {
            /*
                SSSSSSSSSSSSSSS UUUUUUUU     UUUUUUUU   SSSSSSSSSSSSSSS 
                SS:::::::::::::::SU::::::U     U::::::U SS:::::::::::::::S
                S:::::SSSSSS::::::SU::::::U     U::::::US:::::SSSSSS::::::S
                S:::::S     SSSSSSSUU:::::U     U:::::UUS:::::S     SSSSSSS
                S:::::S             U:::::U     U:::::U S:::::S            
                S:::::S             U:::::D     D:::::U S:::::S            
                S::::SSSS          U:::::D     D:::::U  S::::SSSS         
                SS::::::SSSSS     U:::::D     D:::::U   SS::::::SSSSS    
                    SSS::::::::SS   U:::::D     D:::::U     SSS::::::::SS  
                    SSSSSS::::S  U:::::D     D:::::U        SSSSSS::::S 
                            S:::::S U:::::D     D:::::U             S:::::S
                            S:::::S U::::::U   U::::::U             S:::::S
                SSSSSSS     S:::::S U:::::::UUU:::::::U SSSSSSS     S:::::S
                S::::::SSSSSS:::::S  UU:::::::::::::UU  S::::::SSSSSS:::::S
                S:::::::::::::::SS     UU:::::::::UU    S:::::::::::::::SS 
                SSSSSSSSSSSSSSS         UUUUUUUUU       SSSSSSSSSSSSSSS   
                        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣤⣤⣤⣤⣤⣶⣦⣤⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⡿⠛⠉⠙⠛⠛⠛⠛⠻⢿⣿⣷⣤⡀⠀⠀⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⠀⣼⣿⠋⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⠈⢻⣿⣿⡄⠀⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⣸⣿⡏⠀⠀⠀⣠⣶⣾⣿⣿⣿⠿⠿⠿⢿⣿⣿⣿⣄⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⣿⣿⠁⠀⠀⢰⣿⣿⣯⠁⠀⠀⠀⠀⠀⠀⠀⠈⠙⢿⣷⡄⠀
                        ⠀⠀⣀⣤⣴⣶⣶⣿⡟⠀⠀⠀⢸⣿⣿⣿⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣷⠀
                        ⠀⢰⣿⡟⠋⠉⣹⣿⡇⠀⠀⠀⠘⣿⣿⣿⣿⣷⣦⣤⣤⣤⣶⣶⣶⣶⣿⣿⣿⠀
                        ⠀⢸⣿⡇⠀⠀⣿⣿⡇⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠃⠀
                        ⠀⣸⣿⡇⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠉⠻⠿⣿⣿⣿⣿⡿⠿⠿⠛⢻⣿⡇⠀⠀
                        ⠀⣿⣿⠁⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣧⠀⠀
                        ⠀⣿⣿⠀⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀
                        ⠀⣿⣿⠀⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀
                        ⠀⢿⣿⡆⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⡇⠀⠀
                        ⠀⠸⣿⣧⡀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⠃⠀⠀
                        ⠀⠀⠛⢿⣿⣿⣿⣿⣇⠀⠀⠀⠀⠀⣰⣿⣿⣷⣶⣶⣶⣶⠶⠀⢠⣿⣿⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⣿⣿⠀⠀⠀⠀⠀⣿⣿⡇⠀⣽⣿⡏⠁⠀⠀⢸⣿⡇⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⣿⣿⠀⠀⠀⠀⠀⣿⣿⡇⠀⢹⣿⡆⠀⠀⠀⣸⣿⠇⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⢿⣿⣦⣄⣀⣠⣴⣿⣿⠁⠀⠈⠻⣿⣿⣿⣿⡿⠏⠀⠀⠀⠀
                        ⠀⠀⠀⠀⠀⠀⠀⠈⠛⠻⠿⠿⠿⠿⠋⠁⠀⠀⠀
            */
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
        if (playerInfo.VentStanding!=null||playerInfo.inVent)
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
            gameObject.transform.position = new Vector3(40, 8, -86);
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
