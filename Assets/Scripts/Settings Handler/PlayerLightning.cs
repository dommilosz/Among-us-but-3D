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
            if (SabotageScript.GetCurrentSabotage() != null && SabotageScript.GetCurrentSabotage().Type == SabotageScript.Sabotages.FixLights)
            {
                lc.range = (float)(0.2 * lc.range);
            }
        }

        if (!(bool)PlayerInfo.getPlayerInfo().getSetting("Alive"))
        {
            lc.range = 300;
        }

    }
}
