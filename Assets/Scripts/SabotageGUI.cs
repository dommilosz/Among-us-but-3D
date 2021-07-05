﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TaskGUI.EndTask(false);
        }
        if (GameObject.Find("CurrentTask")&& SabotageScript.GetCurrentSabotage().Type==SabotageScript.Sabotages.None)
        {
            TaskGUI.EndTask(true);
        }
    }

}
