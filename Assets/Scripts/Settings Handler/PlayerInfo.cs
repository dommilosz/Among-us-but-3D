using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public List<PlayerProperty> settings = SettingsValues.ReturnDefaultPlayerSettings().ToList();
    public VentScript VentStanding = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        settings = PlayerProperty.checkProps(settings, this);
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
    public PlayerProperty getSettingItem(string name)
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

    float backup_ForwardSpeed;
    float backup_BackwardSpeed;
    float backup_StrafeSpeed;
    float backup_JumpForce;
    public void setCanMove(bool canMove)
    {
        var pm = this.GetComponentInParent<PlayerMovement>();
        if (pm.moveSpeed > 0 && pm.jumpForce > 0)
        {
            backup_ForwardSpeed = pm.moveSpeed;
            backup_JumpForce = pm.jumpForce;
        }
        if (!canMove)
        {
            pm.moveSpeed = 0;
            pm.jumpForce = 0;

            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        else
        {
            pm.moveSpeed = backup_ForwardSpeed;
            pm.jumpForce = backup_JumpForce;
        }

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

    public static PlayerInfo getPlayerInfo()
    {
        return getPlayer().GetComponent<PlayerInfo>();
    }

    public static bool isMine(GameObject player)
    {
        return player.GetComponent<Photon.Pun.PhotonView>().IsMine;
    }
}

