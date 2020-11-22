using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
