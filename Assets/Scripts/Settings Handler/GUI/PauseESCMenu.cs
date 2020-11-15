using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseESCMenu : MonoBehaviour
{
    public GameObject menuPrefab;
    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&active)
        {
            if (GameObject.Find("PauseMenu") == null)
            {
                Show();
            }
            else
            {
                Hide();
            }

            
        }
    }

    public void Show()
    {
        GameObject.Instantiate(menuPrefab).name = "PauseMenu";
        var menu = GameObject.Find("PauseMenu");
        MouseUnLocker.UnlockMouse();
    }

    public void Hide() {
        Destroy(GameObject.Find("PauseMenu"));
        MouseUnLocker.LockMouse();
    }

    public void LeaveRoom()
    {
        SceneManager.LoadScene("AmongUS3D-LobbyScene");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("AmongUS3D-LobbyScene");
        SceneManager.LoadScene("AmongUS3D-LobbyScene");
    }
}
