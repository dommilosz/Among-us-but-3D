using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DBReported : MonoBehaviour
{
    public float TimeToMeeting = 5f;
    public string bodyColor = "red";
    // Start is called before the first frame update
    void Start()
    {

        transform.Find("DBReported").Find("BodyColor").GetComponent<Image>().color = Enums.Colors.getColorByName(bodyColor);
        TimeToMeeting = 5f;
        Hashtable ht = new Hashtable();
        ht.Add("Voted", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    // Update is called once per frame
    void Update()
    {
        MouseUnLocker.UnlockMouse();
        if (TimeToMeeting > 0)
            TimeToMeeting -= Time.deltaTime;
        if (TimeToMeeting < 0) TimeToMeeting = 0;

        if (TimeToMeeting <= 0)
        {
            transform.Find("DBReported").gameObject.SetActive(false);
            transform.Find("MeetingCanvas").gameObject.SetActive(true);
        }
        transform.Find("DBReported").Find("BodyColor").GetComponent<Image>().color = Enums.Colors.getColorByName(bodyColor);
    }
}
