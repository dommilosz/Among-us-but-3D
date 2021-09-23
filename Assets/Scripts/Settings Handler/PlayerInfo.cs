using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public List<SettingProperty> settings = SettingsValues.ReturnDefaultPlayerSettings().ToList();
    public VentScript VentStanding = null;
    public bool canUse = false;
    public TimedAbility MeetingAbility;
    public bool canMove { get { return GetComponent<PlayerMovement>().canMove; } set { GetComponent<PlayerMovement>().canMove = value; } }
    public TaskManager.AllTasks Tasks;
    public bool TasksGenerated = false;

    public Dictionary<string, object> TempData = new Dictionary<string, object>();

    public void SaveTempData()
    {
        Hashtable ht = new Hashtable();
        ht.Add("Temp", TempData);
        getPUNPlayer().SetCustomProperties(ht);
    }

    public void LoadTempData()
    {
        Hashtable ht = getPUNPlayer().CustomProperties;
        TempData = (Dictionary<string, object>)ht["Temp"];
        if (TempData == null)
        {
            TempData = new Dictionary<string, object>();
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        settings = SettingsValues.ReturnDefaultPlayerSettings().ToList();
        InitPlayer();
        CheckColors();
    }

    public void InitPlayer()
    {
        TempData.Clear();
        SaveTempData();
        if (IsMine)
        {
            sendSettings();
            int MeetingsLeft = (int)SettingsHandler.getSetting("Emergency_Count");
            float MeetingsCooldown = (float)SettingsHandler.getSetting("Emergency_Cooldown");
            MeetingAbility = new TimedAbility(TimedCallback.EmptyCallback, MeetingsCooldown);
            MeetingAbility.SetUses(MeetingsLeft);
            MeetingAbility.Start();
            TasksGenerated = false;
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        LoadTempData();
        if (!TasksGenerated)
        {
            Tasks = new TaskManager.AllTasks();
            Tasks.GenerateTasks();
            TasksGenerated = true;
        }
        if (isMine(getPlayer()))
        {
            MeetingAbility.Tick();
        }
        if (getPUNPlayer().IsLocal)
        {
            var tprogress = Tasks.GetTasksProgress();
            TempData["DoneTasksCount"] = tprogress[0];
            TempData["AllTasks"] = tprogress[1];
            SaveTempData();

            if (!IsAlive || InVent)
            {
                IsInvisible = true;
            }
            else if (IsInvisible)
            {
                IsInvisible = false;
            }
        }

        if (SettingProperty.checkProps(settings)) sendSettings();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (getPUNPlayer().UserId == targetPlayer.UserId)
        {
            if (changedProps.ContainsKey("Data") && !(((object[])changedProps["Data"]).Equals(ReturnValues())))
            {
                var settings = (object[])changedProps["Data"];
                LoadValues(settings);
            }
        }
    }

    public void LoadValues(object[] settings)
    {
        int i = 0;
        foreach (var item in settings)
        {
            this.settings[i].setValue(item);
            i++;
        }
    }
    public object[] ReturnValues()
    {
        List<object> values = new List<object>();
        int i = 0;
        foreach (var item in settings)
        {
            values.Add(item.value);
            i++;
        }
        return values.ToArray();
    }

    public object getSetting(string name)
    {
        var item = getSettingItem(name);
        if (item != null)
        {
            return item.value;
        }
        return null;
    }
    public SettingProperty getSettingItem(string name)
    {
        foreach (var item in settings)
        {
            if (item.name == name) return item;
        }
        return null;
    }

    public void setSetting(string name, object value)
    {
        if (!getSetting(name).Equals(value))
        {
            settings[settings.IndexOf(getSettingItem(name))].setValue(value);
            sendSettings();
        }
    }

    public void sendSettings()
    {
        Hashtable ht = new Hashtable();
        ht.Add("Data", ReturnValues());
        getPUNPlayer().SetCustomProperties(ht);
    }

    public static GameObject getPlayer()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (item.GetComponent<Photon.Pun.PhotonView>().IsMine)
            {
                return item;
            }
        }
        return null;
    }

    public Player getPUNPlayer()
    {
        return gameObject.GetComponent<PhotonView>().Owner;
    }

    public static GameObject getPlayerObject(Player player)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players)
        {
            if (item.GetComponent<PlayerInfo>().getPUNPlayer().UserId == player.UserId) return item;
        }
        return null;
    }
    public static PlayerInfo getPlayerInfo(Player player)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players)
        {
            if (item.GetComponent<PlayerInfo>().getPUNPlayer().UserId == player.UserId) return item.GetComponent<PlayerInfo>();
        }
        return null;
    }

    public static PlayerInfo getPlayerInfo()
    {
        if (getPlayer() == null) return null;
        return getPlayer().GetComponent<PlayerInfo>();
    }

    public static bool isMine(GameObject player)
    {
        return player.GetComponent<Photon.Pun.PhotonView>().IsMine;
    }

    public static Player getPlayerByID(string userID)
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == userID) return item;
        }
        return null;
    }
    public static Player getPlayerByName(string name)
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.NickName == name) return item;
        }
        return null;
    }

    public bool IsImpostor
    {
        get
        {
            return (bool)getSetting("isImpostor");
        }
        set
        {
            setSetting("isImpostor",value);
        }
    }

    public bool IsAlive
    {
        get
        {
            return (bool)getSetting("Alive");
        }
        set
        {
            setSetting("Alive", value);
        }
    }

    public bool InVent
    {
        get
        {
            return (bool)getSetting("inVent");
        }
        set
        {
            setSetting("inVent", value);
        }
    }

    public bool IsInvisible
    {
        get
        {
            return (bool)getSetting("Invisible");
        }
        set
        {
            setSetting("Invisible", value);
        }
    }

    public bool IsMine
    {
        get
        {
            return isMine(getPlayer());
        }
    }

    public string Color
    {
        get
        {
            return (string)getSetting("Color");
        }
        set
        {
            setSetting("Color", value);
        }
    }

    public void CheckColors()
    {
        List<string> claimed_colors = new List<string>();
        foreach (var item in PhotonNetwork.PlayerList)
        {
            try
            {
                claimed_colors.Add((string)item.GetPlayerInfo().getSetting("Color"));
            }
            catch { }
        }

        if (claimed_colors.Contains(PhotonNetwork.LocalPlayer.GetPlayerInfo().getSetting("Color")))
        {
            var rndColors = Enums.Colors.AllColors;
            rndColors.Shuffle();

            foreach (var item in rndColors)
            {
                if (!claimed_colors.Contains(item))
                {
                    this.Color = item;
                    break;
                }
            }
        }
    }
}
public static class PlayerExt
{
    public static void changeColor(this Player player, string color)
    {
        player.GetPlayerInfo().setSetting("Color", color);
    }
    public static PlayerInfo GetPlayerInfo(this Player player)
    {
        return PlayerInfo.getPlayerInfo(player);
    }
    public static GameObject GetPlayerObject(this Player player)
    {
        return PlayerInfo.getPlayerObject(player);
    }
}

