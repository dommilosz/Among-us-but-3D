using TMPro;
using UnityEngine;

public class SettingPropertyController : MonoBehaviour
{
    public string propertyName;
    // Start is called before the first frame update
    void Start()
    {
        var value = SettingsHandler.getSettingItem(propertyName).value_str;
        gameObject.transform.Find("SettInput").GetComponent<TMP_InputField>().text = value;
        gameObject.transform.Find("Image").Find("SettName").GetComponent<TextMeshProUGUI>().text = propertyName;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void apply()
    {
        SettingsHandler.setSettingStr(propertyName, gameObject.transform.Find("SettInput").GetComponent<TMP_InputField>().text);
        Start();
    }
}
