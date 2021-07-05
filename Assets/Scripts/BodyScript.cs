using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyScript : MonoBehaviour
{
    public string color;
    public Player player;
    public MeshRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderColor();
    }

    public void RenderColor()
    {
        var mats = _renderer.sharedMaterials;
        mats[0] = Enums.Colors.getMaterial(color);
        _renderer.sharedMaterials = mats;
    }

    private void OnValidate()
    {
        RenderColor();
    }

    public static void DestroyBodies()
    {
        foreach (var item in GameObject.FindObjectsOfType<BodyScript>())
        {
            item.gameObject.Destroy();
        }
    }
}
