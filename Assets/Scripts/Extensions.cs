using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformEx
{
    public static Transform Clear(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    public static Transform Move(this Transform transform,float x,float y,float z)
    {
        transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        return transform;
    }
    
    public static void Destroy(this GameObject go)
    {
        try
        {
            GameObject.Destroy(go);
        }
        catch { }
    }

    public static void PUNDestroy(this GameObject go)
    {
        try
        {
            Photon.Pun.PhotonNetwork.Destroy(go);
        }
        catch { }
    }
}
