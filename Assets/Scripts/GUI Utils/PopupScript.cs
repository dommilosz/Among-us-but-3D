using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PopupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ButtonType
    {
        Info, Warning, Error, OK
    }

    public static Color GetColor(ButtonType type)
    {
        if (type == ButtonType.Info)
        {
            return new Color(0, 0, 0.5f, 0.6f);
        }
        if (type == ButtonType.Warning)
        {
            return new Color(1, 1, 0f, 0.6f);
        }
        if (type == ButtonType.Error)
        {
            return new Color(1, 0, 0f, 0.6f);
        }
        if (type == ButtonType.OK)
        {
            return new Color(0, 1, 0f, 0.6f);
        }
        return new Color(0, 0, 0.5f, 0.6f);
    }

    public static Task<bool> SinglePopup(string content, string btn_text, ButtonType b1type)
    {
        var tcs = new TaskCompletionSource<bool>();
        var single = ShowPopup("Single");
        var contentObj = single.Find("Content").gameObject;
        var buttonObj = single.Find("Button").gameObject;
        contentObj.GetComponent<TMPro.TextMeshProUGUI>().text = content;
        buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = btn_text;
        buttonObj.GetComponent<Image>().color = GetColor(b1type);

        buttonObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            DestroyPopup();
            tcs.SetResult(true);
        });
        return tcs.Task;
    }

    public static Task<bool> YNPopup(string content, string btnY_text, string btnN_text)
    {
        var tcs = new TaskCompletionSource<bool>();
        var single = ShowPopup("YN");
        var contentObj = single.Find("Content").gameObject;
        var buttonYObj = single.Find("ButtonY").gameObject;
        var buttonNObj = single.Find("ButtonN").gameObject;
        contentObj.GetComponent<TMPro.TextMeshProUGUI>().text = content;

        buttonYObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = btnY_text;
        buttonYObj.GetComponent<Image>().color = GetColor(ButtonType.OK);

        buttonNObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = btnN_text;
        buttonNObj.GetComponent<Image>().color = GetColor(ButtonType.Error);

        buttonYObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            DestroyPopup();
            tcs.SetResult(true);
        });

        buttonNObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            DestroyPopup();
            tcs.SetResult(false);
        });

        return tcs.Task;
    }

    public static Transform ShowPopup(string type)
    {
        DestroyPopup();
        GameObject popupObj = Instantiate((GameObject)Resources.Load("Popup"));
        popupObj.name = "UIPopup";
        var popup = popupObj.transform.Find("Canvas").Find(type);
        popup.gameObject.SetActive(true);
        return popup;
    }

    public static void DestroyPopup()
    {
        if (GameObject.Find("UIPopup"))
        {
            GameObject.Find("UIPopup").Destroy();
        }
    }
}
