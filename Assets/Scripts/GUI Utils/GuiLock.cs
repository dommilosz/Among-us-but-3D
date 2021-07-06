using UnityEngine;

public class GuiLock : MonoBehaviour
{
    public bool PreventOpeningGUIs = true;
    public bool UnlockMouse = true;
    public bool Closable = false;
    public bool Destroyable = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MouseUnLocker.MouseLocked = !SholdMouseBeUnlocked();
        if (Closable && Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.Destroy();
        }
    }

    public static bool CanOpenGUI()
    {
        foreach (var item in GameObject.FindObjectsOfType<GuiLock>())
        {
            if (item.PreventOpeningGUIs) return false;
        }
        return true;
    }

    public static bool SholdMouseBeUnlocked()
    {
        foreach (var item in GameObject.FindObjectsOfType<GuiLock>())
        {
            if (item.UnlockMouse) return true;
        }
        return false;
    }

    public static void DestroyOpenGUIs()
    {
        foreach (var item in GameObject.FindObjectsOfType<GuiLock>())
        {
            if(item.Destroyable)
            item.gameObject.Destroy();
        }
    }

    public static GameObject InstantiateGUI(GameObject prefab, bool PreventOpening = true, bool UnlockMouse = true, bool Closable = false)
    {
        if (CanOpenGUI())
        {
            var go = GameObject.Instantiate(prefab);
            var gl = go.AddComponent<GuiLock>();
            gl.Closable = Closable;
            gl.PreventOpeningGUIs = PreventOpening;
            gl.Closable = Closable;
            return go;
        }
        return null;
    }
    public static GameObject InstantiateGUIForce(GameObject prefab, bool PreventOpening = true, bool UnlockMouse = true, bool Closable = false)
    {
        DestroyOpenGUIs();
        var go = GameObject.Instantiate(prefab);
        var gl = go.AddComponent<GuiLock>();
        gl.Closable = Closable;
        gl.PreventOpeningGUIs = PreventOpening;
        gl.Closable = Closable;
        return go;
    }
}
