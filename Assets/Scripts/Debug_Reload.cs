using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Reload : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Photon.Pun.PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                PlayerInfo.getPlayerInfo().Start();
            }
        }
        if (Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 10;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Time.timeScale = 50;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerInfo.getPlayerInfo().setSetting("isImpostor", !PlayerInfo.getPlayerInfo().IsImpostor());
        }
    }
}
