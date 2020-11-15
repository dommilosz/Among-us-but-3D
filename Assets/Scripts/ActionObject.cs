using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (global)
        {
            if (Input.GetKeyDown(Key)) m_event.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Contains("Player") && PlayerInfo.isMine(other.gameObject))
        {
            if (Input.GetKeyDown(Key)) m_event.Invoke();
            other.gameObject.GetComponent<PlayerInfo>().canUse = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player") && PlayerInfo.isMine(other.gameObject))
            other.gameObject.GetComponent<PlayerInfo>().canUse = false;
    }
}
