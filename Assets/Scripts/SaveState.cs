using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveState
{
    public static Dictionary<string, string> PlayerPreferences;
    public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static string amongData = Path.Combine(appdata, "AmongUS-3D");
    public static string preferencesPath = Path.Combine(amongData, "preferences.dat");

    public static void InitState()
    {
        if (!Directory.Exists(amongData))
        {
            Directory.CreateDirectory(amongData);
        }
        ReadState();
    }

    public static void ReadState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            if (File.Exists(preferencesPath))
            {
                var stream = File.OpenRead(preferencesPath);
                PlayerPreferences = (Dictionary<string, string>)bf.Deserialize(stream);
                stream.Close();
            }
        }
        catch
        {

        }

        if (PlayerPreferences == null)
        {
            PlayerPreferences = new Dictionary<string, string>();
        }
    }

    public static void WriteState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        var stream = File.OpenWrite(preferencesPath);
        bf.Serialize(stream, PlayerPreferences);
        stream.Flush();
        stream.Close();
    }
}
