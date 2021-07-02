using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static SabotageScript;

public class LightsSabotageFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sw0 = AmongUsGameManager.GetGameManager().TempData["Switch:0"];
        if (sw0.GetType() == typeof(int) && ((int)sw0) == -1)
        {
            for (int i = 0; i < 5; i++)
            {
                AmongUsGameManager.GetGameManager().TempData["Switch:" + i.ToString()] = UnityEngine.Random.Range(0, 2) == 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool allEnabled = true;
        for (int i = 0; i < 5; i++)
        {
            bool enabled = ((bool)AmongUsGameManager.GetGameManager().TempData.Get("Switch:" + i.ToString(), false));
            if (!enabled) allEnabled = false;
            transform.Find("Switch:" + i.ToString()).GetComponent<Image>().color = enabled ? Color.green : Color.black;
        }
        if (allEnabled && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            SabotageScript.StartSabotage(Sabotages.None, true);
            TaskGUI.EndTask(true);
            this.enabled = false;
        }
    }

    public void ToggleSwitch(int index)
    {
        AmongUsGameManager.GetGameManager().TempData["Switch:" + index.ToString()] = !(bool)AmongUsGameManager.GetGameManager().TempData.Get("Switch:" + index.ToString(), false);
        AmongUsGameManager.GetGameManager().SaveTempData();
    }
}
