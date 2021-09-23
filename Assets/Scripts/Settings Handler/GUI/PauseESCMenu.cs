using Photon.Pun;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.Escape) && active)
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
        var go = GuiLock.InstantiateGUI(menuPrefab, true, true, true);
        if (go != null)
            go.name = "PauseMenu";
    }

    public void Hide()
    {
        Destroy(GameObject.Find("PauseMenu"));
    }

    public void LeaveRoom()
    {
        AmongUsGameManager.Leave();
    }
}
