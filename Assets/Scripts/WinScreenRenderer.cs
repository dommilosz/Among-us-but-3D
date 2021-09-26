using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenRenderer : MonoBehaviour
{
    public Image CrewmatesWon;
    public Image CrewmatesLost;
    public Image ImpostorsWon;
    public Image ImpostorsLost;
    public Image ColorImg;
    // Start is called before the first frame update
    void Start()
    {
        ColorImg.color = PlayerInfo.getPlayerInfo().ColorParsed.color;
        bool impostor = (bool)PlayerInfo.getPlayerInfo().getSetting("isImpostor");
        var gameResult = AmongUsGameManager.GetGameManager().GetGameResult();

        ImpostorsLost.gameObject.SetActive(false);
        CrewmatesWon.gameObject.SetActive(false);
        ImpostorsWon.gameObject.SetActive(false);
        CrewmatesLost.gameObject.SetActive(false);

        if (gameResult == AmongUsGameManager.GameResult.CREWMATES_WON)
        {
            if (impostor)
            {
                ImpostorsLost.gameObject.SetActive(true);
            }
            else
            {
                CrewmatesWon.gameObject.SetActive(true);
            }
        }
        if (gameResult == AmongUsGameManager.GameResult.IMPOSTORS_WON)
        {
            if (impostor)
            {
                ImpostorsWon.gameObject.SetActive(true);
            }
            else
            {
                CrewmatesLost.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MouseUnLocker.MouseLocked = false;

    }

    public void LeaveLobby()
    {
        AmongUsGameManager.Leave();
    }
}
