using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameHUD : MonoBehaviour
{
    public GameObject HUDPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("HUD") == null)
        {
            Instantiate(HUDPrefab).name="HUD";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != "HUD") return;
        var playerInfo = PlayerInfo.getPlayerInfo();
        gameObject.transform.Find("Status").GetComponent<TMPro.TextMeshProUGUI>().text = $"Alive: {playerInfo.IsAlive()}\nImpostor: {playerInfo.IsImpostor()}";
        var taskProgress = playerInfo.Tasks.GetTasksProgress();
        gameObject.transform.Find("LBL_Tasks").GetComponent<TMPro.TextMeshProUGUI>().SetText($"Tasks: {taskProgress[0]}/{taskProgress[1]}", playerInfo.Tasks.Done ? Color.green : Color.white, true);
        var tasksLbl = gameObject.transform.Find("Tasks").GetComponent<TMPro.TextMeshProUGUI>();
        tasksLbl.text = "";
        foreach (var item in playerInfo.Tasks.ShortTasks)
        {
            tasksLbl.AppText(item.name + " (Short)", item.Done ? Color.green : Color.white,true);
        }
        foreach (var item in playerInfo.Tasks.CommonTasks)
        {
            tasksLbl.AppText(item.name + " (Common)", item.Done?Color.green:Color.white,true);
        }
        foreach (var item in playerInfo.Tasks.LongTasks)
        {
            var color = Color.white;
            if(item.Started)
            {
                color = Color.yellow;
            }
            if (item.Done)
            {
                color = Color.green;
            }
            tasksLbl.AppText(item.GetCurrentTask().name + $" {item.GetStringProgress()}" + " (Long)", color,true);
        }
    }
}
