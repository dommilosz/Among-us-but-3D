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

    public static bool MouseLocked
    {
        get
        {
            return _MouseLocked;
        }
        set
        {
            _MouseLocked = value;
            if (_MouseLocked)
            {
                LockMouse();
            }
            else
            {
                UnlockMouse();
            }
        }
    }

    private static bool _MouseLocked = true;

    private static void UnlockMouse()
    {
        var player = PlayerInfo.getPlayer();
        player.GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private static void LockMouse()
    {
        var player = PlayerInfo.getPlayer();
        player.GetComponent<PlayerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
