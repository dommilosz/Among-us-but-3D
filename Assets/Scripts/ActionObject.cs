using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class ActionObject : MonoBehaviour
{
    public string Key;
    [FormerlySerializedAs("onEvent")]
    [SerializeField]
    public UnityEvent m_event;
    public bool global = false;
    public bool ActionEnabled = true;
    public bool OnlyAlive = false;
    public bool OnlyImpostor = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ActionEnabled) return;
        if (global)
        {
            if (Key != null && Input.GetKeyDown(Key)) Execute();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!ActionEnabled) return;
        if (other.gameObject.tag.Contains("Player") && PlayerInfo.isMine(other.gameObject))
        {
            if (Input.GetKeyDown(Key)) Execute();
            other.gameObject.GetComponent<PlayerInfo>().canUse = CanUse();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player") && PlayerInfo.isMine(other.gameObject))
            other.gameObject.GetComponent<PlayerInfo>().canUse = false;
    }

    public void Execute()
    {
        if (!CanUse()) return;
        m_event.Invoke();
    }

    public bool CanUse()
    {
        var pi = PlayerInfo.getPlayerInfo();
        if (OnlyAlive && !pi.IsAlive) return false;
        if (OnlyImpostor && !pi.IsImpostor) return false;
        return true;
    }
}
