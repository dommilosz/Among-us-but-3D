using Photon.Pun;
using UnityEngine;

public class VoteButtonHandler : MonoBehaviour
{
    public MeetingHandler meetingHandler;
    // Start is called before the first frame update
    void Start()
    {
        meetingHandler = GameObject.Find("MeetingCanvas").transform.Find("MeetingCanvas").GetComponent<MeetingHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Vote()
    {
        if (!(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Alive")) return;
        meetingHandler.Vote(gameObject.name.Replace("Meeting_", ""));
    }
}
