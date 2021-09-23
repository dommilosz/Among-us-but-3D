using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AmongUsGameManager;

public class WinScreenShow : MonoBehaviour
{
    public GameObject _WinScreen;
    public static GameObject WinScreen;
    // Start is called before the first frame update
    void Start()
    {
        WinScreen = _WinScreen;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEndGame();
    }

    public static bool GameEnded = false;
    [PunRPC]
    public static void EndGame(GameResult result)
    {
        if (!(bool)SettingsHandler.getSetting("Can_Win")) { return; }
        if (GameEnded) return;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PlayerActions.GetPhotonView().RPC("EndGame", RpcTarget.Others, new object[] { result });
        }
        GameEnded = true;
        WinScreenShow.Show();
    }

    public void CheckEndGame()
    {
        if (!(bool)SettingsHandler.getSetting("Can_Win")) { return; }
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }
        var gr = AmongUsGameManager.GetGameManager().GetGameResult();

        if (gr != GameResult.NONE)
        {
            EndGame(gr);
        }
    }

    public static void Show()
    {
        if (!GameObject.Find("wins"))
        {
            Instantiate(WinScreen).name = "wins";
        }
    }
}
