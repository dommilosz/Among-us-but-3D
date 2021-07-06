using System;
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
        InstantinatedObj.transform.Find("Left").GetComponent<TMPro.TextMeshProUGUI>().text = $"{PlayerInfo.getPlayerInfo().MeetingAbility.UsesLeft} Left. {Math.Round(PlayerInfo.getPlayerInfo().MeetingAbility.RemCooldown)}s Cooldown";

    }
    public GameObject PreMeetingCanvas;
    public GameObject InstantinatedObj;
    public void ShowPreMeeting()
    {
        if (!PlayerInfo.getPlayerInfo().IsAlive()) return;
        if (GameObject.Find("PreMeetingCanvas") != null) return;
        InstantinatedObj = GuiLock.InstantiateGUI(PreMeetingCanvas, true, true, true);
        if (InstantinatedObj != null)
            InstantinatedObj.name = "PreMeetingCanvas";
    }

    public void StartMeeting()
    {
        if (!PlayerInfo.getPlayerInfo().MeetingAbility.Use()) return;
        GameObject.Find("PreMeetingCanvas").Destroy();
        PlayerActions.Meeting();
    }
}
