using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerGUI : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeColor(string color)
    {
        PhotonNetwork.LocalPlayer.changeColor(color);
    }

    public void Show()
    {
        if(!GameObject.Find("ColorPicker"))
        GameObject.Instantiate(prefab).name="ColorPicker";
        MouseUnLocker.UnlockMouse();
    }

    public void Hide()
    {
        if (GameObject.Find("ColorPicker")) Destroy(GameObject.Find("ColorPicker"));
        MouseUnLocker.LockMouse();
    }
}
