using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DBReported : MonoBehaviour
{
    public TimedCallback MeetingCallback;
    public string bodyColor = "red";
    internal bool meeting = false;

    // Start is called before the first frame update
    void Start()
    {

        transform.Find("DBReported").Find("BodyColor").GetComponent<Image>().color = Enums.Colors.getColorObjByName(bodyColor).color;
        MeetingCallback = new TimedCallback(Meeting, 5);
        MeetingCallback.Start();
        Hashtable ht = new Hashtable();
        ht.Add("Voted", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    // Update is called once per frame
    void Update()
    {
        MeetingCallback.Tick();
        transform.Find("DBReported").Find("BodyColor").GetComponent<Image>().color = Enums.Colors.getColorObjByName(bodyColor).color;
        transform.Find("DBReported").Find("Meeting").gameObject.SetActive(meeting);
        transform.Find("DBReported").Find("DeadBodyReported").gameObject.SetActive(!meeting);
    }

    void Meeting()
    {
        transform.Find("DBReported").gameObject.SetActive(false);
        transform.Find("MeetingCanvas").gameObject.SetActive(true);
    }

    private void OnValidate()
    {
        if (Application.isEditor)
        {
            transform.Find("DBReported").Find("BodyColor").GetComponent<Image>().color = Enums.Colors.getColorObjByName(bodyColor).color;
        }
    }
}
