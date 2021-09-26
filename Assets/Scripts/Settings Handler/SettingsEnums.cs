using System.Collections.Generic;
using System.Linq;
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
        public class ColorObj
        {
            public Color color;
            public string name = "";
            public string colorCode
            {
                get
                {
                    int r = Mathf.FloorToInt(color.r*255);
                    int g = Mathf.FloorToInt(color.g*255);
                    int b = Mathf.FloorToInt(color.b*255);

                    return r.ToString("X") + g.ToString("X") + b.ToString("X");
                }
                set
                {
                    int hexInt = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
                    var values = new int[] { (hexInt & 0xFF0000) >> 16, (hexInt & 0x00FF00) >> 8, hexInt & 0x0000FF };

                    color = new Color(values[0] / 255f, values[1] / 255f, values[2] / 255f);
                }
            }

            public ColorObj(string name,Color color)
            {
                this.name = name;
                this.color = color;
            }

            public ColorObj(string name, string code)
            {
                this.name = name;
                this.colorCode = code;
            }
        }

        public static ColorObj[] colors = new ColorObj[] { new ColorObj("red", "BA1010"), new ColorObj("blue", "132DCE"), new ColorObj("green", "107B2B"), new ColorObj("pink", "E752B6"), new ColorObj("orange", "F07D0D"), new ColorObj("yellow", "F6F657"), new ColorObj("gray", "3E464C"), new ColorObj("white", "D7E1F1"), new ColorObj("purple", "6B2FBC"), new ColorObj("brown", "6F471D"), new ColorObj("aqua", "38FFDD"), new ColorObj("lime", "50F039") };

        public static ColorObj getColorObjByName(string name)
        {
            foreach (var item in colors)
            {
                if(item.name == name)
                {
                    return item;
                }
            }
            return null;
        }
        public static Material getMaterial(Color c)
        {
            var mat = new Material(Shader.Find("Standard"));
            mat.color = c;
            return mat;
        }
        public static Material getMaterial(ColorObj obj)
        {
            return getMaterial(obj.color);
        }
        public static Material getMaterial(string color)
        {
            return getMaterial(getColorObjByName(color));
        }
    }
}
