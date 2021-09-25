using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRenderer : MonoBehaviour
{
    public class ChatObject
    {
        public static List<ChatObject> cobjs = new List<ChatObject>();

        public string text;
        public float opacity = 1;
        public float TTL = 5;
        public TMPro.TextMeshProUGUI textMesh;

        public ChatObject(string text, float TTL)
        {
            this.text = text;
            this.TTL = TTL;
            cobjs.Add(this);
        }

        public bool Tick()
        {
            var opacityChange = Time.deltaTime;
            opacityChange = opacityChange / TTL;

            opacity -= opacityChange;
            if (opacity <= 0)
            {
                opacity = 0;
            }
            var oldColor = textMesh.color;
            oldColor.a = opacity;
            textMesh.color = oldColor;
            if(opacity <= 0)
            {
                cobjs.Remove(this);
                textMesh.gameObject.Destroy();
                return true;
            }
            return false;
        }

        public GameObject Create(GameObject parent)
        {
            var obj = Instantiate((GameObject)Resources.Load("Chat Entry"));
            obj.transform.SetParent(parent.transform);
            textMesh = obj.GetComponent<TMPro.TextMeshProUGUI>();
            textMesh.text = this.text;
            return obj;
        }

        public static void AddMsg(string message, GameObject parent, float TTL = 15f)
        {
            var obj = new ChatObject(message, TTL);
            obj.Create(parent);
        }

        public static void TickAll()
        {
            foreach (var item in cobjs)
            {
                if (item.Tick()) break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChatObject.TickAll();
    }

    public static ChatRenderer GetChatRenderer()
    {
        return (ChatRenderer)GameObject.FindObjectOfType(typeof(ChatRenderer));
    }

    public void AddMsg(string text, float TTL = 15f)
    {
        ChatObject.AddMsg(text, gameObject, TTL);
    }
}
