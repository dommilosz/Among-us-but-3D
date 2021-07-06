using UnityEngine;
using UnityEngine.UI;
using static SabotageScript;

public class CommsSabotageFix : MonoBehaviour
{
    public RadialValueSelect rvs;
    public Image statusLight;
    [Range(0, 360)]
    public int DegreesPerCheck = 9;

    // Start is called before the first frame update
    void Start()
    {
        if ((int)AmongUsGameManager.GetGameManager().TempData["CommsSabotage"] == -1)
            AmongUsGameManager.GetGameManager().TempData["CommsSabotage"] = Random.Range(((rvs.rangeMin) / DegreesPerCheck).Floor() + 1, ((rvs.rangeMax) / DegreesPerCheck).Floor() - 1);
        AmongUsGameManager.GetGameManager().SaveTempData();
    }

    // Update is called once per frame
    void Update()
    {
        if ((((int)rvs.value) / DegreesPerCheck) == (int)AmongUsGameManager.GetGameManager().TempData.Get("CommsSabotage", 0))
        {
            statusLight.color = Color.black;
        }
        else
        {
            statusLight.color = Color.red;
        }
        if ((((int)rvs.value) / DegreesPerCheck) == (int)AmongUsGameManager.GetGameManager().TempData.Get("CommsSabotage", 0) && !rvs.isRotating)
        {
            statusLight.color = Color.green;
            SabotageScript.StartSabotage(Sabotages.None, true);
            TaskGUI.EndTask(true);
            this.enabled = false;
        }
    }
}
