using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TaskManager;

public class TaskObj : MonoBehaviour
{
    public string TaskName = "";
    public TaskManager.Task task;
    ActionObject ao;

    // Start is called before the first frame update
    void Start()
    {
        ao = gameObject.AddComponent<ActionObject>();
        ao.Key = "e";
        ao.m_event = new UnityEngine.Events.UnityEvent();
        ao.m_event.AddListener(StartTask);
        ao.ActionEnabled = false;
    }

    // Update is called once per frame
    void Update()
    { 
        bool canDo = CanDoTask();
        ao.ActionEnabled = canDo;
        GetComponent<Outline>().enabled = canDo;
    }

    public void StartTask()
    {
        if (GameObject.Find("CurrentTask")){
            GameObject.Find("CurrentTask").Destroy();
        }
        var task = Instantiate(TaskManager.Task.GetByName(TaskName).prefab);
        task.name = "CurrentTask";
        var tgui = task.GetComponent<TaskGUI>();
        tgui.TaskName= TaskName;
        tgui.task = this;
        MouseUnLocker.UnlockMouse();
        PlayerInfo.getPlayerInfo().canMove = false;
    }

    public bool CanDoTask() {
        if (PlayerInfo.getPlayerInfo().IsImpostor()) return false;
        if (!task.Active) return false;
        if (task.Done) return false;
        if (task.TaskType == TaskTypes.Long && task.GetLongTask().GetCurrentTask() != task) return false;
        return true;
    }

    private void OnValidate()
    {
        task.name = TaskName;
    }
}