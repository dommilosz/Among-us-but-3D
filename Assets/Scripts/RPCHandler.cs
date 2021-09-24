using Discord;
using Photon.Pun;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class RPCHandler : MonoBehaviour
{
    public long applicationId = 890690524191809546;
    public Discord.Discord discord;
    TimedAbility RpcUpdateTimer;

    void Start()
    {
        RpcUpdateTimer = new TimedAbility(ChangeRPC, 2);
        RpcUpdateTimer.enabled = true;
    }

    void Update()
    {
        discord.RunCallbacks();
        RpcUpdateTimer.Tick();
        RpcUpdateTimer.Use();
    }

    string LastState = "";
    Activity presence = new Discord.Activity();
    public static string RPCRoomName = "";
    public static bool IsGameJoinPending
    {
        get
        {
            return RPCRoomName != "";
        }
    }

    public void JoinGame(string secret)
    {
        RPCRoomName = secret.atob().Split(':')[1].atob();
    }

    public static string GetGameToJoin()
    {
        var tmp = RPCRoomName;
        RPCRoomName = "";
        return tmp;
    }

    async void ChangeRPC()
    {
        Debug.Log("Update discord RPC");
        var cr = PhotonNetwork.CurrentRoom;
        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));

        presence.Name = "Among US 3d";
        presence.Type = Discord.ActivityType.Playing;

        if (PhotonNetwork.InRoom)
        {
            presence.Party.Size.CurrentSize = cr.PlayerCount;
            presence.Party.Size.MaxSize = cr.MaxPlayers;
            presence.Party.Id = cr.Name;
            presence.Secrets.Join = ("Join:"+cr.Name.btoa()).btoa();
            presence.Secrets.Match = ("Match:"+cr.Name.btoa()).btoa();
            presence.Secrets.Spectate = ("Spectate:"+cr.Name.btoa()).btoa();

            var pl = PhotonNetwork.LocalPlayer;
            try
            {
                var pi = pl.GetPlayerInfo();
                if (cr.CustomProperties.Get("Started", false))
                {
                    presence.State = "In Game";
                    presence.Details = $"Playing Among US but 3d. Color: ({pi.Color})";
                }
                else
                {
                    presence.State = "In Lobby";
                    presence.Details = $"Playing Among US but 3d. Color: ({pi.Color})";
                }
            }
            catch
            {

            }



        }
        else
        {
            presence.Party = new Discord.ActivityParty();
            presence.Secrets = new Discord.ActivitySecrets();
            presence.State = "In main menu";
            presence.Details = $"Playing Among US but 3d";
        }

        if (LastState != presence.State)
        {
            presence.Timestamps.Start = (long)timeSpan.TotalSeconds;
        }
        LastState = presence.State;

        try
        {
            await UpdateActivityAsync(presence);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        
    }

    Task<Result> UpdateActivityAsync(Activity activity)
    {
        var tcs = new TaskCompletionSource<Result>();
        var activityManager = discord.GetActivityManager();
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Result.Ok)
            {
                tcs.SetException(new Exception("Discord RPC Exception: "+res.ToString()));
            }
            else
            {
                tcs.SetResult(res);
            }
            
        });
        return tcs.Task;
    }

    void OnEnable()
    {
        Debug.Log("Discord: init");
        discord = new Discord.Discord(applicationId, (ulong)Discord.CreateFlags.NoRequireDiscord);
        var am = discord.GetActivityManager();
        am.OnActivityJoin += JoinGame;
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        discord.Dispose();
    }

    void OnDestroy()
    {

    }
}
