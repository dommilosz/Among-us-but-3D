using UnityEngine;

public class VentScript : MonoBehaviour
{
    public GameObject ventCanvas;
    GameObject SpawnedVentCanvas;
    public GameObject nextVent;
    public GameObject prevVent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var playerInfo = PlayerInfo.getPlayerInfo();
        if (Input.GetKeyDown(KeyCode.F) && playerInfo.VentStanding == this && !(bool)playerInfo.getSetting("inVent"))
        {
            enterVent();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F) && playerInfo.VentStanding == this && (bool)playerInfo.getSetting("inVent"))
        {
            exitVent();
            return;
        }
        if (Input.GetKeyDown(KeyCode.A) && playerInfo.VentStanding == this && (bool)playerInfo.getSetting("inVent"))
        {
            ventPrev();
            return;
        }
        if (Input.GetKeyDown(KeyCode.D) && playerInfo.VentStanding == this && (bool)playerInfo.getSetting("inVent"))
        {
            ventNext();
            return;
        }
        if (SpawnedVentCanvas != null && !(bool)playerInfo.getSetting("inVent"))
        {
            Destroy(SpawnedVentCanvas);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.tag.Contains("Player") && PlayerInfo.isMine(collision.gameObject))
        {
            var player = collision.gameObject;
            var playerInfo = collision.gameObject.GetComponent<PlayerInfo>();
            if ((bool)playerInfo.getSetting("isImpostor"))
            {
                playerInfo.VentStanding = this;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!PlayerInfo.isMine(collision.gameObject)) return;
        var playerInfo = collision.gameObject.GetComponent<PlayerInfo>();
        playerInfo.VentStanding = null;
    }

    public void enterVent()
    {
        enterVentS(gameObject);
    }
    public void exitVent()
    {
        exitVentS(gameObject);
    }

    public static void enterVentS(GameObject vent)
    {
        var ventscr = vent.GetComponent<VentScript>();
        if (ventscr.SpawnedVentCanvas == null)
            ventscr.SpawnedVentCanvas = GameObject.Instantiate(ventscr.ventCanvas);
        var playerInfo = PlayerInfo.getPlayerInfo();
        var player = PlayerInfo.getPlayer();
        playerInfo.setSetting("inVent", true);
        playerInfo.canMove = false;

        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        tpPlayerToVent(vent);
    }

    public static void exitVentS(GameObject vent)
    {
        var ventscr = vent.GetComponent<VentScript>();
        var playerInfo = PlayerInfo.getPlayerInfo();

        Destroy(ventscr.SpawnedVentCanvas);
        playerInfo.setSetting("inVent", false);
        playerInfo.canMove = true;
    }
    public void ventNext()
    {
        if (nextVent == null) ventPrev();
        tpPlayerToVent(nextVent);
    }
    public void ventPrev()
    {
        if (prevVent == null) ventNext();
        tpPlayerToVent(prevVent);
    }

    public static void tpPlayerToVent(GameObject vent)
    {
        var player = PlayerInfo.getPlayer();
        player.transform.position = vent.transform.position;
        player.transform.rotation = vent.transform.rotation;

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);

    }
}
