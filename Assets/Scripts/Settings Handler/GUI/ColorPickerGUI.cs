using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerGUI : MonoBehaviour
{
    public GameObject prefab;
    public List<string> claimed_colors = new List<string>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        claimed_colors.Clear();
        if (transform.Find("Colors") != null)
        {
            for (int i = 0; i < transform.Find("Colors").childCount; i++)
            {
                var c = transform.Find("Colors").GetChild(i);
                if (c.name.Contains("Color"))
                {
                    c.transform.Find("off").gameObject.SetActive(false);
                }
            }
            foreach (var item in PhotonNetwork.PlayerList)
            {
                var obj = GameObject.Find("Color_" + (string)item.GetPlayerInfo().getSetting("Color"));
                obj.transform.Find("off").gameObject.SetActive(true);
                claimed_colors.Add((string)item.GetPlayerInfo().getSetting("Color"));
            }
        }
    }

    public void changeColor(string color)
    {
        if (!claimed_colors.Contains(color))
            PhotonNetwork.LocalPlayer.changeColor(color);
    }

    public void Show()
    {
        var cp = GuiLock.InstantiateGUI(prefab, true, true, true);
        if (cp != null)
            cp.name = "ColorPicker";
    }

    public void Hide()
    {
        if (GameObject.Find("ColorPicker")) Destroy(GameObject.Find("ColorPicker"));
    }
}
