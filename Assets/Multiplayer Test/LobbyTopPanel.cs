using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class LobbyTopPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";
    private readonly string playerNameMessage = "PlayerName : ";

    [Header("UI References")]
    public Text ConnectionStatusText;
    public Text PlayerNameText;

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
        if(PhotonNetwork.LocalPlayer!=null&& PhotonNetwork.LocalPlayer.NickName != null&& PhotonNetwork.LocalPlayer.NickName.Length>0)
        {
            PlayerNameText.text = playerNameMessage + PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            PlayerNameText.text = "";
        }
    }

    #endregion
}
