using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var lc = gameObject.GetComponent<Light>();
        if ((bool)PlayerInfo.getPlayerInfo().getSetting("isImpostor"))
        {
            lc.range = (float)SettingsHandler.getSetting("ImpostorVision");
        }
        else
        {
            lc.range = (float)SettingsHandler.getSetting("PlayerVision");
        }

        if (!(bool)PlayerInfo.getPlayerInfo().getSetting("Alive"))
        {
            lc.range = 300;
        }

    }
}
