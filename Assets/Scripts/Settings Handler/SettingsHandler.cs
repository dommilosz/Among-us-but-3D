using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public string Map = Enums.Maps.Polus;
    public int Impostors = 2;

    public int Emergency_Count = 1;
    public float Emergency_Cooldown = 30;

    public float KillCooldown = 45f;
    public float KillDistance = 10f;

    public float PlayerSpeed = 8f;
    public float PlayerVision = 50f;
    public float ImpostorVision = 150f;

    public int DiscusionTime = 15;
    public int VotingTime = 90;
    public bool Confirm_Ejects = true;
    public bool Anonymous_Votes = true;

    public int Short_Tasks = 2;
    public int Long_Tasks = 2;
    public int Common_Tasks = 2;
    public bool Visual_Tasks = true;
    public string TaskBar_Updates = Enums.TaskbarUpdates.Always;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static SettingsHandler getSettings()
    {
        return GameObject.Find("Settings Handler").GetComponent<SettingsHandler>();
    }
}
