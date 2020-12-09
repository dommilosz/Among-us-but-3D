using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScript : MonoBehaviour
{
    public float KillCd;
    // Start is called before the first frame update
    void Start()
    {
        KillCd = (float)SettingsHandler.getSetting("KillCooldown");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        // if raycast hits, it checks if it hit an object with the tag Player
        if (Physics.Raycast(transform.Find("Orientation").position, transform.Find("Orientation").forward, out hit, (int)SettingsHandler.getSetting("KillDistance")) && hit.collider.gameObject.CompareTag("Player"))
        {
            SelectedPlayer = hit.transform.gameObject.GetComponent<PhotonView>().Owner.UserId;
        }
        else
        {
            SelectedPlayer = "";
        }

        if (KillCd > 0)
        {
            KillCd -= Time.deltaTime;
        }
        if (KillCd < 0)
        {
            KillCd = 0;
        }
    }
    public void KillClosestPlayer()
    {
        if (!canKill()) return;
        if (SelectedPlayer.Length <= 0) return;
        var p = PlayerInfo.getPlayerByID(SelectedPlayer);
        if (!canKillPlayer(p)) return;
        KillCd = (float)SettingsHandler.getSetting("KillCooldown");
        KillPlayer(p);
        
    }
    public void KillPlayer(Player player)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("KillPlayer", RpcTarget.All, new object[] { player.UserId,PhotonNetwork.LocalPlayer });
    }

    public bool canKill()
    {
        if (!(bool)PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("isImpostor")) return false;
        if (KillCd <= 0) return true;
        return false;
    }

    public Player getClosestPlayer()
    {
        float minDist = -1;
        Player minPlayer = null;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (getDistanceToPlayer(item) < minDist || minDist < 0)
            {
                minDist = getDistanceToPlayer(item);
                minPlayer = item;
            }
        }
        return minPlayer;
    }

    public float getDistanceToPlayer(Player player)
    {
        return Vector3.Distance(player.GetPlayerObject().transform.position, transform.position);
    }

    public bool canKillPlayer(Player p)
    {
        if (getDistanceToPlayer(p) <= (int)SettingsHandler.getSetting("KillDistance")) return true;
        return false;
    }

    public GameObject KillParticleSystem;
    public GameObject KilledByCanvas;
    public GameObject bodyPrefab;

    public string SelectedPlayer;

    [PunRPC]
    public void KillPlayer(string playerID,Player sender)
    {
        if (PhotonNetwork.LocalPlayer.UserId == playerID)
        {
            PhotonNetwork.LocalPlayer.GetPlayerInfo().setSetting("Alive", false);
            GameObject kbc = GameObject.Instantiate(KilledByCanvas);
            var color = (string)sender.GetPlayerInfo().getSetting("Color");
            var colorCode = Enums.Colors.getColorCodeByName(color);

            kbc.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"Killed by <color={colorCode}>{color}</color>";
            kbc.Destroy(5);
        }
        var player = PlayerInfo.getPlayerByID(playerID);
        var playerObj = player.GetPlayerObject();
        var x = playerObj.transform.position.x;
        var y = playerObj.transform.position.y;
        var z = playerObj.transform.position.z;

        var kps = GameObject.Instantiate(KillParticleSystem, new Vector3(x, y, z), Quaternion.identity);
        GameObject.Destroy(kps, 5);

        var body = GameObject.Instantiate(bodyPrefab, new Vector3(x, y, z), Quaternion.identity);
        body.name = $"Body {playerID}";
        body.tag = "Body";
        body.transform.SetParent(GameObject.Find("Players").transform);

    }
}
