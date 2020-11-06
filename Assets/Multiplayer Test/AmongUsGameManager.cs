using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUsGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("PlayerV2", transform.position, transform.rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)
    }

    // Update is called once per frame
    void Update()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var item in players)
        {
            if (!PlayerInfo.isMine(item))
            {
                if(item.transform.Find("PlayerCamera")!=null&& item.transform.Find("Point Light") != null)
                {
                    Destroy(item.transform.Find("PlayerCamera").gameObject);
                    Destroy(item.transform.Find("Point Light").gameObject);

                    item.GetComponent<PlayerActions>().enabled = false;
                }
            }
        }
    }
}
