using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUnLocker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UnlockMouse()
    {
        var player = PlayerInfo.getPlayer();
        player.GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void LockMouse()
    {
        var player = PlayerInfo.getPlayer();
        player.GetComponent<PlayerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
