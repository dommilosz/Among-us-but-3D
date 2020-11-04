using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var lc = gameObject.GetComponent<Light>();
        lc.range = SettingsHandler.getSettings().PlayerVision;
    }
}
