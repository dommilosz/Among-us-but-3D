using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SabotageScript;

public class SabotageObj : MonoBehaviour
{
    public Sabotages SabotageType;
    public GameObject UIPrefab;
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

    public bool CanDoTask()
    {
        if (SabotageScript.GetCurrentSabotage().Type!=SabotageType) return false;
        return true;
    }

    public void StartTask()
    {
        if (GameObject.Find("CurrentTask"))
        {
            GameObject.Find("CurrentTask").Destroy();
        }
        var task = Instantiate(UIPrefab);
        task.name = "CurrentTask";
        MouseUnLocker.UnlockMouse();
        PlayerInfo.getPlayerInfo().canMove = false;
    }
}
