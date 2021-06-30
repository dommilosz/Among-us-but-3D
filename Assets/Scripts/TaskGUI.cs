using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGUI : MonoBehaviour
{
    public string TaskName = "";
    public TaskObj task;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndTask(false);
        }
    }

    public void EndTask(bool success)
    {
        if (success)
        {
            if (!PlayerInfo.getPlayerInfo().Tasks.CompletedTasks.Contains(task.task))
            {
                PlayerInfo.getPlayerInfo().Tasks.CompletedTasks.Add(task.task);
            }
            task.task.Done = true;
        }
        if (GameObject.Find("CurrentTask"))
        {
            GameObject.Find("CurrentTask").Destroy();
        }
        MouseUnLocker.LockMouse();
        PlayerInfo.getPlayerInfo().canMove = true;
    }

    public void ResetTask()
    {
        task.StartTask();
    }
}
