using UnityEngine;
using UnityEngine.SceneManagement;
using static SabotageScript;

public class MapScript : MonoBehaviour
{
    public GameObject pointer;
    public float canvasW = 500;
    public float canvasH = 500;
    public float mapW = 1;
    public float mapH = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var player = PlayerInfo.getPlayer();
        var ppos = player.transform.position;

        var parent = transform.Find(SceneManager.GetActiveScene().name.Replace("AmongUs_", ""));
        pointer.transform.SetParent(parent);

        if (PlayerInfo.getPlayerInfo().IsImpostor())
        {
            parent.Find("Normal").gameObject.SetActive(false);
            parent.Find("Sabotage").gameObject.SetActive(true);
        }
        else
        {
            parent.Find("Sabotage").gameObject.SetActive(false);
            parent.Find("Normal").gameObject.SetActive(true);
        }

        var xRatio = ppos.x / mapW;
        var yRatio = ppos.z / mapH;

        pointer.transform.localPosition = new Vector3(xRatio * canvasW, yRatio * canvasH, 0);
    }

    public void StartSabotage(Sabotages sabotage, float TimeLeft)
    {
        SabotageScript.StartSabotage(sabotage, TimeLeft);
    }
}
