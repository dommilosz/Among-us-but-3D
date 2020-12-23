using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript: MonoBehaviour
{
    public GameObject pointer;
    public float canvasW = 500;
    public float canvasH = 500;
    public float mapW = 1;
    public float mapH = 1;

    public bool sabotage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = PlayerInfo.getPlayer();
        var ppos = player.transform.position;

        var parent = transform.Find(SceneManager.GetActiveScene().name.Replace("AmongUs_", ""));
        pointer.transform.SetParent(parent);
        parent.Find("Sabotage").gameObject.SetActive(false);
        parent.Find("Normal").gameObject.SetActive(true);

        if (sabotage)
        {
            parent.Find("Normal").gameObject.SetActive(false);
            parent.Find("Sabotage").gameObject.SetActive(true);
        }

        var xRatio = ppos.x / mapW;
        var yRatio = ppos.z / mapH;

        pointer.transform.localPosition = new Vector3(xRatio*canvasW, yRatio * canvasH, 0);
    }
}
