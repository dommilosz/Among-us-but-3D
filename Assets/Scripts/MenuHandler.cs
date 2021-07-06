using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseAction()
    {
        PlayerActions.UseAction();
    }
    public void SabotageAction()
    {
        PlayerActions.SabotageAction();
    }
    public void VentAction()
    {
        PlayerActions.VentAction();
    }

    public void ReportAction(string color)
    {
        PlayerActions.ReportAction(color);
    }

    public void KillAction()
    {
        PlayerActions.KillAction();
    }
}
