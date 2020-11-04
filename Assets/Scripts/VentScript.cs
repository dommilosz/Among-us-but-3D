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
        if(SpawnedVentCanvas==null)
        SpawnedVentCanvas = GameObject.Instantiate(ventCanvas);
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        playerInfo.inVent = true;
        playerInfo.setCanMove(false);
    }
    public void exitVent()
    {
        var playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();

        Destroy(SpawnedVentCanvas);
        playerInfo.inVent = false;
        playerInfo.setCanMove(true);
    }
    public void ventNext()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = nextVent.transform.position;
        player.transform.rotation = nextVent.transform.rotation;

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z);
    }
    public void ventPrev()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = prevVent.transform.position;
        player.transform.rotation = prevVent.transform.rotation;

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z);
    }
}
