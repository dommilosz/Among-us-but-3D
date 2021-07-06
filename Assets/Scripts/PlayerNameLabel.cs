using Photon.Pun;
using UnityEngine;

public class PlayerNameLabel : MonoBehaviour
{
    public GameObject labelPrefab;
    public float YOffset = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        var label = GameObject.Instantiate(labelPrefab, transform);
        label.name = gameObject.GetComponent<PlayerInfo>().getPUNPlayer().NickName + "- [label]";
        label.GetComponent<TMPro.TextMeshPro>().text = gameObject.GetComponent<PlayerInfo>().getPUNPlayer().NickName;
        label.transform.localPosition = new Vector3(0, YOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var label = getLabel();
        if (label != null)
        {
            var mainCam = PhotonNetwork.LocalPlayer.GetPlayerObject().transform.Find("PlayerCamera");
            label.transform.rotation = mainCam.rotation;
        }
    }

    public GameObject getLabel()
    {
        return GameObject.Find(gameObject.GetComponent<PlayerInfo>().getPUNPlayer().NickName + "- [label]");
    }
}
