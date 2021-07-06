using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static SabotageScript;

public class ReactorSabotageFix : MonoBehaviour
{
    public byte ThisState = 1;
    public Image image;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool enabled1 = false;
        bool enabled2 = false;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            var pi = item.GetPlayerInfo();
            if ((byte)pi.TempData.Get("ReactorState", 0) == 1)
            {
                enabled1 = true;
            }
            if ((byte)pi.TempData.Get("ReactorState", 0) == 2)
            {
                enabled2 = true;
            }
            if (enabled1 && enabled2) break;
        }
        if (enabled1 && enabled2)
        {
            SabotageScript.StartSabotage(Sabotages.None, true);
            TaskGUI.EndTask(true);
            this.enabled = false;
        }

        if (image.GetComponent<IsMouseOn>().isMouseOn && Input.GetMouseButton(0))
        {
            SetState(ThisState);
        }
        else
        {
            SetState(0);
        }
        var pil = PlayerInfo.getPlayerInfo();
        image.color = ((byte)pil.TempData["ReactorState"] == ThisState) ? Color.green : Color.red;

    }

    public static void SetState(byte state)
    {
        var pi = PlayerInfo.getPlayerInfo();
        pi.TempData["ReactorState"] = state;
        pi.SaveTempData();
    }
}
