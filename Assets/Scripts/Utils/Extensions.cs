using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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

    public static void SetRectWidth(this RectTransform transform,float width)
    {
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width);
    }
    public static void SetRectHeight(this RectTransform transform,float height)
    {
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height);
    }

    public static byte[] ReadAllBytes(this Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(bytes, 0, (int)stream.Length);
        return bytes;
    }
    public static void WriteAllBytes(this Stream stream,byte[] bytes)
    {
        stream.Write(bytes, 0, bytes.Length);
    }

    public static void AppText(this TMPro.TextMeshProUGUI TMP,string text,Color c, bool newline=false)
    {
        var colorCode = BitConverter.ToString(new byte[] { (byte)(c.r*255), (byte)(c.g*255), (byte)(c.b*255) }).Replace("-", string.Empty); ;
        TMP.text += $"<color=#{colorCode}>{text}</color>"+ (newline ? "\n": String.Empty);
    }
    public static void AppText(this TMPro.TextMeshProUGUI TMP, string text, bool newline=false)
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
}