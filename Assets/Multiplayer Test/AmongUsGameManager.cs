using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUsGameManager : MonoBehaviour
{
    public GameObject playersPlaceHolder;
    // Start is called before the first frame update
    void Start()
    {
        var player = PhotonNetwork.Instantiate("PlayerV2", transform.position, transform.rotation, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var item in players)
        {
            item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.UserId;
            item.transform.parent = playersPlaceHolder.transform;
            if (!PlayerInfo.isMine(item))
            {
                if(item.transform.Find("PlayerCamera")!=null&& item.transform.Find("Point Light") != null)
                {
                    Destroy(item.transform.Find("PlayerCamera").gameObject);
                    Destroy(item.transform.Find("Point Light").gameObject);

                    item.GetComponent<PlayerActions>().enabled = false;
                }
            }
            else
            {
                item.name = "Player " + item.GetComponent<Photon.Pun.PhotonView>().Controller.NickName+ " (ME)";
            }
        }
    }
}
