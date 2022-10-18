using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnScreenReticleXboxGameBar
{
    internal class JsonParser
    {
        Settings settings;
        JsonSerializer serializer;

        public JsonParser()
        {
            DeserializeSettings();
        }

        public void SerializeSettings()
        {
            serializer.NullValueHandling = NullValueHandling.Ignore;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OnScreenReticle\OnScreenReticle.json"; ;
            using (StreamWriter sw = new StreamWriter(path))
            {
                serializer.Serialize(sw, settings);
            }
        }

        public void DeserializeSettings()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OnScreenReticle";
            Directory.CreateDirectory(path);
            string fullPath = Path.Combine(path, "OnScreenReticle.json");

            if (File.Exists(fullPath))
            {
                string json = "";
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    json = sr.ReadToEnd();
                }

                settings = JsonConvert.DeserializeObject<Settings>(json);
            }

            if (settings == null)
            {
                settings = new Settings();
            }
        }
    }

    public class Settings
    {
        public Settings()
        {
            Top = 250;
            Left = 125;

            DotDiameter = 5;
            DotColorA = 255;
            DotColorR = 255;
            DotColorG = 0;
            DotColorB = 0;
            DotVisibility = true;

            AngleThickness = 5;
            AngleLength = 30;
            AngleAngle = 90;
            AngleColorA = 255;
            AngleColorR = 255;
            AngleColorG = 0;
            AngleColorB = 0;
            AngleVisibility = true;

            CrossThickness = 5;
            CrossLength = 30;
            CrossOffset = 15;
            CrossAngle = 0;
            CrossColorA = 255;
            CrossColorR = 255;
            CrossColorG = 0;
            CrossColorB = 0;
            CrossVisibility = true; 
        }

        public int Top { get; set; }
        public int Left { get; set; }

        // Dot
        public double DotDiameter { get; set; }
        public int DotColorA { get; set; }
        public int DotColorR { get; set; }
        public int DotColorG { get; set; }
        public int DotColorB { get; set; }
        public bool DotVisibility { get; set; }

        // Angle
        public double AngleThickness { get; set; }
        public double AngleLength { get; set; }
        public double AngleAngle { get; set; }
        public int AngleColorA { get; set; }
        public int AngleColorR { get; set; }
        public int AngleColorG { get; set; }
        public int AngleColorB { get; set; }
        public bool AngleVisibility { get; set; }

        // Cross
        public double CrossThickness { get; set; }
        public double CrossLength { get; set; }
        public double CrossOffset { get; set; }
        public double CrossRotation { get; set; }
        public int CrossColorA { get; set; }
        public int CrossColorR { get; set; }
        public int CrossColorG { get; set; }
        public int CrossColorB { get; set; }
        public bool CrossVisibility { get; set; }
    }
}
