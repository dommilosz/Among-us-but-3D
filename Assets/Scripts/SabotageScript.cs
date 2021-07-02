using Photon.Pun;
using System;
using UnityEngine;

public class SabotageScript : MonoBehaviour
{
    public TimedAbility SabotageAbility;
    public Sabotage CurrentSabotage;
    public Sabotages ToRunSabotageType;
    public bool RunSabotage = false;
    // Start is called before the first frame update
    void Start()
    {
        SabotageAbility = new TimedAbility(TimedCallback.EmptyCallback, 30);
        SabotageAbility.Start();
        CurrentSabotage = new Sabotage(Sabotages.None, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetCurrentSabotage().Type != Sabotages.None)
        {
            SabotageAbility.Reset();
        }
        SabotageAbility.Tick();

        CurrentSabotage.Type = (Sabotages)AmongUsGameManager.GetGameManager().TempData.Get("SabotageType", Sabotages.None);
        CurrentSabotage.TimeLeft = (float)AmongUsGameManager.GetGameManager().TempData.Get("SabotageTime", -1f);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (CurrentSabotage.TimeLeft != -1)
            {
                CurrentSabotage.TimeLeft -= Time.deltaTime;
                AmongUsGameManager.GetGameManager().TempData["SabotageTime"] = CurrentSabotage.TimeLeft;
            }
        }
        if (CurrentSabotage.TimeLeft < 0 && CurrentSabotage.TimeLeft != -1)
        {

        }
    }

    public static void StartSabotage(Sabotages type,bool force=false)
    {
        float TimeLeft = -1f;
        switch (type)
        {
            case Sabotages.FixLights:
                TimeLeft = -1;break;
            case Sabotages.Reactor:
                TimeLeft = 60f;break;
            case Sabotages.Comms:
                TimeLeft = -1; break;
            case Sabotages.None:
                break;
            default:
                break;
        }
        StartSabotage(type, TimeLeft,force);
    }

    public enum Sabotages
    {
        FixLights, Reactor, Comms, None
    }

    public static Sabotage GetCurrentSabotage()
    {
        var sabotageScript = GetSabotageScript();
        if (sabotageScript == null) return null;
        return sabotageScript.CurrentSabotage;
    }

    public static void StartSabotage(Sabotages sabotage, float TimeLeft, bool force = false)
    {
        var sabotageScript = GetSabotageScript();
        if (force || sabotageScript.SabotageAbility.Use())
        {
            AmongUsGameManager.GetGameManager().TempData["SabotageType"] = sabotage;
            AmongUsGameManager.GetGameManager().TempData["SabotageTime"] = TimeLeft;

            AmongUsGameManager.GetGameManager().TempData["CommsSabotage"] = -1;
            AmongUsGameManager.GetGameManager().TempData["Switch:0"] = -1;
            AmongUsGameManager.GetGameManager().SaveTempData();
        }
        
    }

    public static SabotageScript GetSabotageScript()
    {
        return AmongUsGameManager.GetGameManager().GetComponent<SabotageScript>();
    }

    public class Sabotage
    {
        public float TimeLeft;
        public Sabotages Type
        {
            get { return _Type; }
            set
            {
                if (value == Sabotages.Reactor) critical = true; _Type = value;
            }
        }
        Sabotages _Type;
        public bool critical = false;

        public Sabotage(Sabotages type, float TimeLeft)
        {
            if (type == Sabotages.Reactor) critical = true;
        }

        new public string ToString()
        {
            if (Type == Sabotages.None)
            {
                return "";
            }
            if (TimeLeft == -1)
            {
                return $"{Type.ToString("G")}";
            }
            return $"{Type.ToString("G")}: {TimeLeft.Floor()}s";
        }
    }

    public void OnValidate()
    {
        if (RunSabotage)
        {
            RunSabotage = false;
            StartSabotage(ToRunSabotageType, -1, true);
        }
    }
}
