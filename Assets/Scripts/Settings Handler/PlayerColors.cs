using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColors : MonoBehaviour
{
    public MaterialObj[] colors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var playerInfo = gameObject.GetComponent<PlayerInfo>();
        foreach (var item in colors)
        {
            if (((string)playerInfo.getSetting("Color")).ToLower() == item.name)
            {
                playerInfo.gameObject.GetComponent<MeshRenderer>().material = item.material;
            }
        }
       
    }
}

[Serializable]
public class MaterialObj
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Material material;
}
