using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
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

    // Start is called before the first frame update
    public void Start()
    {
        settings = SettingsValues.ReturnDefaultPlayerSettings().ToList();
        if (isMine(getPlayer()))
        {
            sendSettings();
            int MeetingsLeft = (int)SettingsHandler.getSetting("Emergency_Count");
            float MeetingsCooldown = (float)SettingsHandler.getSetting("Emergency_Cooldown");
            MeetingAbility = new TimedAbility(TimedCallback.EmptyCallback, MeetingsCooldown);
            MeetingAbility.SetUses(MeetingsLeft);
            MeetingAbility.Start();
            TasksGenerated = false;
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
                foreach (var item in Enums.Colors.AllColors)
                {
                    if (!claimed_colors.Contains(item))
                    {
                        setSetting("Color", item);
                        break;
                    }
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
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
            setSetting("DoneTasks", Tasks.CompletedTasks.Count);
            if (!(bool)getSetting("Alive") || (bool)getSetting("inVent"))
            {
                setSetting("Invisible", true);
            }
            else if ((bool)getSetting("Invisible"))
            {
                setSetting("Invisible", false);
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

    [PunRPC]
    public void AddCommonTasks(byte[] cTasks)
    {
        TaskManager.AllTasks.AddCommonTasks(cTasks);
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

    public bool IsImpostor()
    {
        return (bool)getSetting("isImpostor");
    }

    public bool IsAlive()
    {
        return (bool)getSetting("Alive");
    }
}
public static class PlayerExt
{
    public static void changeColor(this Player player,string color)
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

