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

    }

    public void _EndTask(bool success)
    {
        if (success)
        {
            if (!PlayerInfo.getPlayerInfo().Tasks.CompletedTasks.Contains(task.task))
            {
                PlayerInfo.getPlayerInfo().Tasks.CompletedTasks.Add(task.task);
            }
            task.task.Done = true;
        }
        EndTask(success);
        PlayerInfo.getPlayerInfo().canMove = true;
    }

    public static void EndTask(bool success)
    {
        if (GameObject.Find("CurrentTask"))
        {
            if (success) GameObject.Find("CurrentTask").Destroy(1.5f);
            else GameObject.Find("CurrentTask").Destroy();
        }
        PlayerInfo.getPlayerInfo().canMove = true;
    }

    public void ResetTask()
    {
        task.StartTask();
    }
}
