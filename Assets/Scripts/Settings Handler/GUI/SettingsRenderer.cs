using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsRenderer : MonoBehaviour
{
    public GameObject propPrefab_Editable;
    public GameObject propPrefab;
    public GameObject content;
    public GameObject content_edit_prefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(content!=null)
        DrawUneditable();
    }

    public void DrawUneditable()
    {
        var values = SettingsHandler.getSettings().settings;
        int y = 20;
        content.transform.Clear();
        foreach (var item in values)
        {
            DrawAt(y, item.name, item.value_str);
            y -= 20;
        }
    }

    public void DrawEditable()
    {
        if (!GameObject.Find("Settings_Editor"))
        {
            var tmp = GameObject.Instantiate(content_edit_prefab);
            tmp.name = "Settings_Editor";
        }
        var Settings_Editor = GameObject.Find("Settings_Editor");
        var cnt = Settings_Editor.transform.Find("Image").Find("Scroll View").Find("Viewport").Find("Content");
        cnt.Clear();

        var values = SettingsHandler.getSettings().settings;
        int y = -35;

        foreach (var item in values)
        {
            DrawEditableAt(y, item.name, item.value_str);
            y -=50;
        }
        cnt.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (-y));

    }

    public void DrawEditableAt(float y, string name, string value)
    {
        var Settings_Editor = GameObject.Find("Settings_Editor");
        var prop = Instantiate(propPrefab_Editable);

        prop.GetComponent<SettingPropertyController>().propertyName = name;

        prop.transform.parent = Settings_Editor.transform.Find("Image").Find("Scroll View").Find("Viewport").Find("Content");
        prop.transform.localPosition = new Vector3(0, y, 0);
    }
    public void DrawAt(float y, string name, string value)
    {
        var prop = Instantiate(propPrefab);
        prop.GetComponent<TextMeshProUGUI>().text = $"{name} : {value}";

        prop.transform.SetParent(content.transform);
        prop.transform.localPosition = new Vector3(0, y, 0);
    }

    public void Show()
    {
        if (GameObject.Find("Settings_Editor")) return;
        MouseUnLocker.UnlockMouse();
        DrawEditable();
    }

    public void Hide()
    {
        MouseUnLocker.LockMouse();
        var Settings_Editor = GameObject.Find("Settings_Editor");
        Destroy(Settings_Editor);
    }
}
