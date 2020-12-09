using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enums : MonoBehaviour
{
    public static class Maps
    {
        public static string Polus = "polus";
        public static string MiraHQ = "mirahq";
        public static string Skeld = "skeld";
    }
    public static class TaskbarUpdates
    {
        public static string Always = "Always";
        public static string Never = "Never";
        public static string Meetings = "Meetings";
    }
    public static class Colors
    {
        public static string[] AllColors = new string[] { "red","blue","green","pink","orange","yellow","gray","white","purple","brown","aqua","lime"};
        public static string[] AllColorsCodes = new string[] { "#BA1010","#132DCE","#107B2B", "#E752B6", "#F07D0D", "#F6F657", "#3E464C", "#D7E1F1", "#6B2FBC", "#6F471D", "#38FFDD", "#50F039" };

        public static string getColorCodeByName(string code)
        {
            try
            {
                return AllColorsCodes[AllColors.ToList().IndexOf(code)];
            }
            catch { return ""; }
        }

        public static Color getColorByName(string color)
        {
            var values = getColorValuesByName(color);
            return new Color((float)values[0]/255, (float)values[1]/255, (float)values[2]/255);
        }

        public static int[] getColorValuesByName(string color)
        {
            var hex = getColorCodeByNameNoHash(color);
            int hexInt = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

            return new int[] { (hexInt & 0xFF0000) >> 16, (hexInt & 0x00FF00) >> 8, hexInt & 0x0000FF };
        }

        public static string getColorCodeByNameNoHash(string color)
        {
            return getColorCodeByName(color).Replace("#","");
        }
    }
}
