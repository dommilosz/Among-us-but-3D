using Photon.Pun;
using System;
using UnityEngine;

public class PlayerColors : MonoBehaviour
{
    public Shader OutlineShader;
    public Shader TransparencyShader;
    public Color mainColor = new Color(255, 0, 0);
    public Color outlineColor = new Color(0, 0, 0);
    public float outlineWidth = 0.007f;

    // Start is called before the first frame update
    void Start()
    {
        var playerInfo = gameObject.GetComponent<PlayerInfo>();
        var mats = playerInfo.gameObject.transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials;

        playerInfo.gameObject.transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials = mats;
    }

    // Update is called once per frame
    void Update()
    {
        var playerInfo = gameObject.GetComponent<PlayerInfo>();
        outlineWidth = 0.007f;
        outlineColor = new Color(0, 0, 0);
        mainColor = Enums.Colors.getColorByName((string)playerInfo.getSetting("Color"));

        if ((bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("isImpostor") && PhotonNetwork.LocalPlayer.GetPlayerObject().GetComponent<KillScript>().SelectedPlayer == gameObject.GetComponent<PhotonView>().Owner.UserId)
        {
            outlineColor = new Color(0.8f, 0, 0);
            outlineWidth = 0.02f;
        }

        var mats = transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials;

        if (!(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Alive") && !(bool)playerInfo.getSetting("Alive"))
        {
            mainColor.a = 0.3f;

            mats[0] = new Material(TransparencyShader);
            mats[0].SetColor("_Color", mainColor);
        }
        else
        {
            mats[0] = new Material(OutlineShader);
            mats[0].SetColor("_Color", mainColor);
            mats[0].SetColor("_OutlineColor", outlineColor);
            mats[0].SetFloat("_Outline", outlineWidth);
        }


        transform.Find("Orientation").Find("playerbestmodel").Find("Cube").GetComponent<SkinnedMeshRenderer>().materials = mats;



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
