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
        if (SabotageScript.GetCurrentSabotage() != null && SabotageScript.GetCurrentSabotage().Type == SabotageScript.Sabotages.Comms)
        {
            GetComponent<Outline>().enabled = false;
        }

    }

    public void StartTask()
    {
        var task = GuiLock.InstantiateGUI(TaskManager.Task.GetByName(TaskName).prefab, true, true, true);
        if (task == null) return;
        task.name = "CurrentTask";
        var tgui = task.GetComponent<TaskGUI>();
        tgui.TaskName = TaskName;
        tgui.task = this;
        PlayerInfo.getPlayerInfo().canMove = false;
    }

    public bool CanDoTask()
    {
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