using UnityEngine;
using static SabotageScript;

public class SabotageObj : MonoBehaviour
{
    public Sabotages SabotageType;
    public GameObject UIPrefab;
    ActionObject ao;
    public TimedCallback OutlineChanger;

    // Start is called before the first frame update
    void Start()
    {
        ao = gameObject.AddComponent<ActionObject>();
        ao.Key = "e";
        ao.m_event = new UnityEngine.Events.UnityEvent();
        ao.m_event.AddListener(StartTask);
        ao.ActionEnabled = false;
        OutlineChanger = new TimedCallback(ChangeOutline, 0.5f);
        OutlineChanger.repeat = true;
        OutlineChanger.Start();
    }

    // Update is called once per frame
    void Update()
    {
        OutlineChanger.Tick();
        bool canDo = CanDoTask();
        ao.ActionEnabled = canDo;
        GetComponent<Outline>().enabled = canDo;
    }

    public bool OutlineState = false;
    public void ChangeOutline()
    {
        var outline = GetComponent<Outline>();
        outline.OutlineColor = OutlineState ? Color.yellow : Color.red;
        OutlineState = !OutlineState;
    }

    public bool CanDoTask()
    {
        if (SabotageScript.GetCurrentSabotage().Type != SabotageType) return false;
        return true;
    }

    public void StartTask()
    {
        var task = GuiLock.InstantiateGUI(UIPrefab, true, true, true);
        if (task != null)
            task.name = "CurrentTask";
    }
}
