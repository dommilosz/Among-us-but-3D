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
        
    }
}
