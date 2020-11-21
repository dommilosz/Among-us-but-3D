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
                if (playerInfo.gameObject.GetComponent<MeshRenderer>() != null)
                {
                    playerInfo.gameObject.GetComponent<MeshRenderer>().material = item.material;

                }
                else
                {
                    //playerInfo.gameObject.transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials[0] = item.material;
                    {
                        var mats = playerInfo.gameObject.transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials;
                        mats[0] = item.material;
                        playerInfo.gameObject.transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials = mats;
                    }
                }
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
