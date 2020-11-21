using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameLabel : MonoBehaviour
{
    public GameObject labelPrefab;
    public float YOffset = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        var label = GameObject.Instantiate(labelPrefab,transform);
        label.name = gameObject.GetComponent<PlayerInfo>().getPUNPlayer().NickName + "- [label]";
        label.GetComponent<TMPro.TextMeshPro>().text = gameObject.GetComponent<PlayerInfo>().getPUNPlayer().NickName;
        label.transform.localPosition = new Vector3(0, YOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var mainCam = PhotonNetwork.LocalPlayer.GetPlayerObject().transform.Find("PlayerCamera");
        getLabel().transform.rotation = mainCam.rotation;
    }

    public GameObject getLabel()
    {
        return GameObject.Find(gameObject.GetComponent<PlayerInfo>().getPUNPlayer().NickName + "- [label]");
    }
}
