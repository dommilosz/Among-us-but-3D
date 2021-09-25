using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetSize();
    }

    void OnValidate()
    {
        //SetSize();
    }

    public void SetSize()
    {
        var children = gameObject.GetComponentsInChildren(typeof(RectTransform));
        float maxW = 0;
        float maxH = 0;
        foreach (var item in children)
        {
            var transform = (RectTransform)item;
            var rect = transform.rect;
            if (maxH < rect.y + rect.height) maxH = rect.y + rect.height;
            if (maxW < rect.x + rect.width) maxW = rect.x + rect.width;
        }
        gameObject.GetComponent<RectTransform>().SetRectWidth(maxW);
        gameObject.GetComponent<RectTransform>().SetRectHeight(maxH);
    }
}
