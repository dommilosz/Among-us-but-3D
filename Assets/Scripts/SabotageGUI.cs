using UnityEngine;

public class SabotageGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var currentTask = GameObject.Find("CurrentTask");
        if (currentTask && SabotageScript.GetCurrentSabotage().Type == SabotageScript.Sabotages.None)
        {
            TaskGUI.EndTask(true);
        }
    }

}
