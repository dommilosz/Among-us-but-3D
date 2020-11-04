using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript: MonoBehaviour
{
    GameObject pointer;
    public float offsetX = 1;
    public float offsetY = 1;
    // Start is called before the first frame update
    void Start()
    {
        pointer = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        pointer.transform.localPosition = new Vector3((-player.transform.position.x)*offsetX, (-player.transform.position.z)* offsetY, 0);
    }
}
