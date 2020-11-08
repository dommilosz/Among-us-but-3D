using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SettingsHandler : MonoBehaviourPunCallbacks
{
    public List<SettingProperty> settings = SettingsValues.ReturnDefaultSettings().ToList();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        settings = SettingProperty.checkProps(settings);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("Settings"))
        {
            var settings = (object[])PhotonNetwork.CurrentRoom.CustomProperties["Settings"];
            LoadValues(settings);
        }

        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }
    public static SettingsHandler getSettings()
    {
        return GameObject.Find("Settings Handler").GetComponent<SettingsHandler>();
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

    public static object getSetting(string name)
    {
        var item = getSettingItem(name);
        if (item != null)
        {
            return item.value;
        }
        return null;
    }
    public static SettingProperty getSettingItem(string name)
    {
        foreach (var item in getSettings().settings)
        {
            if (item.name == name) return item;
        }
        return null;
    }

    public static void setSetting(string name, object value)
    {
        getSettings().settings[getSettings().settings.IndexOf(getSettingItem(name))].setValue(value);
        sendSettings();
    }

    public static void sendSettings()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Settings", getSettings().ReturnValues());
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
    }
}

public static class SettingsValues
{
    public static SettingProperty[] ReturnDefaultSettings()
    {
        List<SettingProperty> properties = new List<SettingProperty>();

        properties.Add(new SettingProperty("Map", Enums.Maps.Polus));
        properties.Add(new SettingProperty("Impostors", 2));

        properties.Add(new SettingProperty("Emergency_Count", 1));
        properties.Add(new SettingProperty("Emergency_Cooldown", 30));

        properties.Add(new SettingProperty("KillCooldown", 45f));
        properties.Add(new SettingProperty("KillDistance", 10f));

        properties.Add(new SettingProperty("PlayerSpeed", 8f));
        properties.Add(new SettingProperty("PlayerVision", 50f));
        properties.Add(new SettingProperty("ImpostorVision", 150f));

        properties.Add(new SettingProperty("DiscusionTime", 15));
        properties.Add(new SettingProperty("VotingTime", 90));
        properties.Add(new SettingProperty("Confirm_Ejects", true));
        properties.Add(new SettingProperty("Anonymous_Votes", true));

        properties.Add(new SettingProperty("Short_Tasks", 2));
        properties.Add(new SettingProperty("Long_Tasks", 2));
        properties.Add(new SettingProperty("Common_Tasks", 2));
        properties.Add(new SettingProperty("Visual_Tasks", true));
        properties.Add(new SettingProperty("TaskBar_Updates", Enums.TaskbarUpdates.Always));

        return properties.ToArray();
    }

    internal static PlayerProperty[] ReturnDefaultPlayerSettings()
    {
        List<PlayerProperty> properties = new List<PlayerProperty>();

        properties.Add(new PlayerProperty("Color", Enums.Colors.RED));
        properties.Add(new PlayerProperty("isImpostor", false));
        properties.Add(new PlayerProperty("inVent", false));
        properties.Add(new PlayerProperty("PlayerName", false));

        return properties.ToArray();
    }
}

[Serializable]
public class SettingProperty
{
    [SerializeField]
    public string name;
    [SerializeField]
    public object value { get; private set; }
    [SerializeField]
    public string value_str;
    public SettingProperty() { }
    public SettingProperty(string _name, object _value) { name = _name; value = _value; value_str = value.ToString(); }

    public object parseValue(string obj)
    {
        if (value is int) return Convert.ToInt32(obj);
        if (value is float) return float.Parse(obj);
        if (value is bool) return Convert.ToBoolean(obj);

        return obj;
    }
    public void setValue(object value)
    {
        this.value = value;
        this.value_str = this.value.ToString();
    }
    public static List<SettingProperty> checkProps(List<SettingProperty> props)
    {
        bool changed = false;
        foreach (var item in props)
        {
            try
            {
                if (item.value.ToString() == item.value_str)
                {

                }
                else
                {
                    item.setValue(item.parseValue(item.value_str));
                    changed = true;
                }
            }
            catch { }
            item.value_str = item.value.ToString();
        }
        if (changed)
        {
            SettingsHandler.sendSettings();
        }
        return props;
    }
}

[Serializable]
public class PlayerProperty
{
    [SerializeField]
    public string name;
    [SerializeField]
    public object value { get; private set; }
    [SerializeField]
    public string value_str;
    public PlayerProperty() { }
    public PlayerProperty(string _name, object _value) { name = _name; value = _value; value_str = value.ToString(); }

    public object parseValue(string obj)
    {
        if (value is int) return Convert.ToInt32(obj);
        if (value is float) return float.Parse(obj);
        if (value is bool) return Convert.ToBoolean(obj);

        return obj;
    }
    public void setValue(object value)
    {
        this.value = value;
        this.value_str = this.value.ToString();
    }
    public static List<PlayerProperty> checkProps(List<PlayerProperty> props,PlayerInfo info)
    {
        bool changed = false;
        foreach (var item in props)
        {
            try
            {
                if (item.value.ToString() == item.value_str)
                {

                }
                else
                {
                    item.setValue(item.parseValue(item.value_str));
                    changed = true;
                }
            }
            catch { }
            item.value_str = item.value.ToString();
        }
        if (changed)
        {
            info.sendSettings();
        }
        return props;
    }
}