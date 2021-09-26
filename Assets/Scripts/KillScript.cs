using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class KillScript : MonoBehaviour
{
    public TimedAbility KillAbility;
    // Start is called before the first frame update
    void Start()
    {
        KillAbility = new TimedAbility(KillAction, (float)SettingsHandler.getSetting("KillCooldown"));
        KillAbility.Start();
    }

    // Update is called once per frame
    void Update()
    {
        KillAbility.Tick();
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
    }
    public void KillClosestPlayer()
    {
        if (!IsImpostor()) return;
        if (SelectedPlayer.Length <= 0) return;
        var p = PlayerInfo.getPlayerByID(SelectedPlayer);
        if (!canKillPlayer(p)) return;

        if (KillAbility.action == null) KillAbility.action = KillAction;

        KillAbility.Use();
    }

    public void KillAction()
    {
        var p = PlayerInfo.getPlayerByID(SelectedPlayer);
        KillPlayer(p);
    }

    public void KillPlayer(Player player)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("KillPlayer", RpcTarget.All, new object[] { player, PhotonNetwork.LocalPlayer });
    }

    public bool IsImpostor()
    {
        return PhotonNetwork.LocalPlayer.GetPlayerInfo().IsImpostor;
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
        if (p.GetPlayerInfo().IsImpostor) return false;
        if (getDistanceToPlayer(p) <= (int)SettingsHandler.getSetting("KillDistance")) return true;
        return false;
    }

    public GameObject KillParticleSystem;
    public GameObject KilledByCanvas;
    public GameObject bodyPrefab;

    public string SelectedPlayer;

    [PunRPC]
    public void KillPlayer(Player deadPlayer, Player sender)
    {
        if (PhotonNetwork.LocalPlayer.UserId == deadPlayer.UserId)
        {
            PhotonNetwork.LocalPlayer.GetPlayerInfo().setSetting("Alive", false);
            GameObject kbc = GameObject.Instantiate(KilledByCanvas);
            var color = (string)sender.GetPlayerInfo().getSetting("Color");
            var colorCode = Enums.Colors.getColorObjByName(color).colorCode;

            kbc.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"Killed by <color={colorCode}>{color}</color>";
            kbc.Destroy(5);
        }
        var playerObj = deadPlayer.GetPlayerObject();
        var x = playerObj.transform.position.x;
        var y = playerObj.transform.position.y;
        var z = playerObj.transform.position.z;

        var kps = GameObject.Instantiate(KillParticleSystem, new Vector3(x, y, z), Quaternion.identity);
        GameObject.Destroy(kps, 5);

        var body = GameObject.Instantiate(bodyPrefab, new Vector3(x, y, z), Quaternion.identity);
        string deadColor = (string)deadPlayer.GetPlayerInfo().getSetting("Color");

        body.name = $"Body {deadPlayer.NickName}";
        var bs = body.GetComponent<BodyScript>();
        bs.player = deadPlayer;
        bs.color = deadColor;
        body.tag = "Body";

    }
}
