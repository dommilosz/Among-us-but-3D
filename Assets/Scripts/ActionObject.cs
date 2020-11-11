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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            if (Input.GetKeyDown(Key)) m_event.Invoke();
            other.gameObject.GetComponent<PlayerInfo>().canUse = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
            other.gameObject.GetComponent<PlayerInfo>().canUse = false;
    }
}
