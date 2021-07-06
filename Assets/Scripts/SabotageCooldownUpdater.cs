using UnityEngine;
using UnityEngine.UI;

public class SabotageCooldownUpdater : MonoBehaviour
{
    public SabotageScript.Sabotages Type;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Type.ToString("G") + "\n" + SabotageScript.GetSabotageScript().SabotageAbility.RemCooldown.Floor();
        GetComponent<Button>().onClick.AddListener(() => SabotageScript.StartSabotage(Type));
    }
}
