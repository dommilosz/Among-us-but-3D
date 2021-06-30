using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InstantinatedObj == null) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InstantinatedObj.gameObject.Destroy();
        }
        InstantinatedObj.transform.Find("Left").GetComponent<TMPro.TextMeshProUGUI>().text = $"{PlayerInfo.getPlayerInfo().MeetingAbility.UsesLeft} Left. {Math.Round(PlayerInfo.getPlayerInfo().MeetingAbility.RemCooldown)}s Cooldown";

    }
    public GameObject PreMeetingCanvas;
    public GameObject InstantinatedObj;
    public void ShowPreMeeting()
    {
        if (!PlayerInfo.getPlayerInfo().IsAlive()) return;
        if (GameObject.Find("PreMeetingCanvas") != null) return;
        InstantinatedObj = Instantiate(PreMeetingCanvas);
        InstantinatedObj.name = "PreMeetingCanvas";
        MouseUnLocker.UnlockMouse();
    }

    public void StartMeeting()
    {
        if (!PlayerInfo.getPlayerInfo().MeetingAbility.Use()) return;
        GameObject.Find("PreMeetingCanvas").Destroy();
        PlayerActions.Meeting();
    }
}
