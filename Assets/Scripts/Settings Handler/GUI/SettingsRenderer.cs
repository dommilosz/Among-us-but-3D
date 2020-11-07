using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsRenderer : MonoBehaviour
{
    public GameObject propPrefab_Editable;
    public GameObject propPrefab;
    public GameObject content;
    public bool isEdit = false;
    // Start is called before the first frame update
    void Start()
    {
        DrawAll();
    }

    // Update is called once per frame
    void Update()
    {
        DrawAll();
    }

    public void DrawEditableAt(float y,string name,string value)
    {
        var prop = Instantiate(propPrefab_Editable);
        prop.transform.Find("SettInput").GetComponent<TMP_InputField>().text = value;
        prop.transform.Find("Image").Find("SettName").GetComponent<TextMeshProUGUI>().text = name;

        prop.transform.parent = content.transform;
        prop.transform.localPosition = new Vector3(0, y, 0);
    }
    public void DrawAt(float y, string name, string value)
    {
        var prop = Instantiate(propPrefab);
        prop.GetComponent<TextMeshProUGUI>().text = $"{name} : {value}";

        prop.transform.parent = content.transform;
        prop.transform.localPosition = new Vector3(0, y, 0);
    }

    public void DrawAll()
    {
        var values = SettingsHandler.getSettings().settings;
        int y = isEdit ? -20 : 20;
        content.transform.Clear();
        foreach (var item in values)
        {
            if (isEdit) DrawEditableAt(y, item.name, item.value_str);
            if (!isEdit) DrawAt(y, item.name, item.value_str);

            y -= isEdit ? 40 : 15;
        }
    }
}
