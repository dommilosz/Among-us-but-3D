using System.Collections;
using System.Collections.Generic;
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
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        if (Input.GetKeyDown(KeyCode.F)&&playerInfo.VentStanding==this&&!playerInfo.inVent)
        {
            enterVent();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F) && playerInfo.VentStanding == this&& playerInfo.inVent)
        {
            exitVent();
            return;
        }
        if (Input.GetKeyDown(KeyCode.A) && playerInfo.VentStanding == this && playerInfo.inVent)
        {
            ventPrev();
            return;
        }
        if (Input.GetKeyDown(KeyCode.D) && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>().VentStanding == this && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>().inVent)
        {
            ventNext();
            return;
        }
        if (SpawnedVentCanvas != null&&!playerInfo.inVent)
        {
            Destroy(SpawnedVentCanvas);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag.Contains("Player"))
        {
            var player = collision.collider.gameObject;
            var playerInfo = collision.collider.gameObject.GetComponent<PlayerInfo>();
            if (playerInfo.isImpostor)
            {
                playerInfo.VentStanding = this;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var playerInfo = collision.collider.gameObject.GetComponent<PlayerInfo>();
        playerInfo.VentStanding = null;
    }

    public void enterVent()
    {
        enterVentS(gameObject);
    }public void exitVent()
    {
        exitVentS(gameObject);
    }

    public static void enterVentS(GameObject vent)
    {
        var ventscr = vent.GetComponent<VentScript>();
        if (ventscr.SpawnedVentCanvas == null)
            ventscr.SpawnedVentCanvas = GameObject.Instantiate(ventscr.ventCanvas);
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        var player = GameObject.FindGameObjectWithTag("Player");
        playerInfo.inVent = true;
        playerInfo.setCanMove(false);

        tpPlayerToVent(vent);
    }

    public static void exitVentS(GameObject vent)
    {
        var ventscr = vent.GetComponent<VentScript>();
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();

        Destroy(ventscr.SpawnedVentCanvas);
        playerInfo.inVent = false;
        playerInfo.setCanMove(true);
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
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = vent.transform.position;
        player.transform.rotation = vent.transform.rotation;

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);

    }
}
