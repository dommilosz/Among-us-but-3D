﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public static class UtilsExtensions
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<T> Clone<T>(this IList<T> list)
    {
        return list.ToArray().ToList();
    }

    public static void SetRectWidth(this RectTransform transform, float width)
    {
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
    public static void SetRectHeight(this RectTransform transform, float height)
    {
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public static byte[] ReadAllBytes(this Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(bytes, 0, (int)stream.Length);
        return bytes;
    }
    public static void WriteAllBytes(this Stream stream, byte[] bytes)
    {
        stream.Write(bytes, 0, bytes.Length);
    }

    public static void AppText(this TMPro.TextMeshProUGUI TMP, string text, Color c, bool newline = false)
    {
        var colorCode = BitConverter.ToString(new byte[] { (byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255) }).Replace("-", string.Empty); ;
        TMP.text += $"<color=#{colorCode}>{text}</color>" + (newline ? "\n" : String.Empty);
    }
    public static void AppText(this TMPro.TextMeshProUGUI TMP, string text, bool newline = false)
    {
        TMP.text += text + (newline ? "\n" : String.Empty);
    }
    public static void SetText(this TMPro.TextMeshProUGUI TMP, string text, Color c, bool newline = false)
    {
        var colorCode = BitConverter.ToString(new byte[] { (byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255) }).Replace("-", string.Empty); ;
        TMP.text = $"<color=#{colorCode}>{text}</color>" + (newline ? "\n" : String.Empty);
    }
    public static void SetText(this TMPro.TextMeshProUGUI TMP, string text, bool newline = false)
    {
        TMP.text = text + (newline ? "\n" : String.Empty);
    }

    public static int Floor(this float f)
    {
        return (int)Math.Floor(f);
    }

    public static U Get<T, U>(this Dictionary<T, U> dict, T key, U _default)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        else
        {
            return _default;
        }
    }

    public static U Get<U>(this ExitGames.Client.Photon.Hashtable dict, string key, U _default)
    {
        if (dict.ContainsKey(key))
        {
            return (U)dict[key];
        }
        else
        {
            return _default;
        }
    }

    public static string btoa(this string str)
    {
        return System.Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(str));
    }

    public static string atob(this string str)
    {
        return ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(str));
    }

    public static void DontDestroyUntil0(this GameObject ga)
    {
        void Destroy(Scene current, Scene next)
        {
            if (next.buildIndex == 0)
            {
                ga.Destroy();
            }
        }
        GameObject.DontDestroyOnLoad(ga);
        SceneManager.activeSceneChanged += Destroy;
    }
}