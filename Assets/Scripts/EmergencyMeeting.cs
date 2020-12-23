using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyMeeting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.Destroy();
        }
        transform.Find("Left").GetComponent<TMPro.TextMeshProUGUI>().text = $"{PlayerInfo.getPlayerInfo().MeetingsLeft} Left. {Math.Round(PlayerInfo.getPlayerInfo().MeetingsCooldown)}s Cooldown";
    }

    public void DoEmergencyMeeting()
    {
        if (PlayerInfo.getPlayerInfo().MeetingsLeft <= 0) return;
        if (PlayerInfo.getPlayerInfo().MeetingsCooldown > 0) return;
        PlayerActions.ReportAction("meeting");
        gameObject.Destroy();
        PlayerInfo.getPlayerInfo().MeetingsCooldown = (float)SettingsHandler.getSetting("Emergency_Cooldown");
        PlayerInfo.getPlayerInfo().MeetingsLeft -= 1;
    }

    public static void ShowMeeting(GameObject ButtonCanvas)
    {
        Instantiate(ButtonCanvas);
        MouseUnLocker.UnlockMouse();
    }
}
