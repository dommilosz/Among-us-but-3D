using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadTaskScript : MonoBehaviour
{
    TimedCallback timer;
    public Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        timer = new TimedCallback(TimedCallback.EmptyCallback,7);
    }

    // Update is called once per frame
    void Update()
    {
        timer.Tick();
        if (timer.executed)
        {
            gameObject.GetComponent<TaskGUI>()._EndTask(true);
            this.enabled = false;
        }
        var parentW = progressBar.transform.parent.GetComponent<RectTransform>().rect.width;
        var ratio = (float)(7 - timer.RemDelay) / 7;
        progressBar.GetComponent<RectTransform>().SetRectWidth(ratio * parentW);
    }

    public void StartDownloading()
    {
        timer.Start();
    }
}
