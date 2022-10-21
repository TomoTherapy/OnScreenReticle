using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnScreenReticleXboxGameBar
{
    public class JsonParser
    {
        private SettingsList settingsList;
        private JsonSerializer serializer;

        public SettingsList SettingsList { get => settingsList; set => settingsList = value; }

        public JsonParser()
        {
            serializer = new JsonSerializer();
            DeserializeSettings();
        }

        public void SerializeSettings()
        {
            serializer.NullValueHandling = NullValueHandling.Ignore;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OnScreenReticle\OnScreenReticle.json";
            //string path = @"D:\OnScreenReticle\OnScreenReticle.json";
            using (StreamWriter sw = new StreamWriter(path))
            {
                serializer.Serialize(sw, settingsList);
            }
        }

        public void DeserializeSettings()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OnScreenReticle";
            //string path = @"D:\OnScreenReticle";
            //Directory.CreateDirectory(path);
            string fullPath = Path.Combine(path, "OnScreenReticle.json");

            if (File.Exists(fullPath))
            {
                string json = "";
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    json = sr.ReadToEnd();
                }

                settingsList = JsonConvert.DeserializeObject<SettingsList>(json);
            }

            if (settingsList == null)
            {
                settingsList = new SettingsList() { ChosenOne = 0 };
                settingsList.List.Add(new Settings() { Name = "Default" });
            }
        }
    }

    public class SettingsList
    {
        public int ChosenOne { get; set; }
        public List<Settings> List { get; set; }

        public SettingsList()
        {
            List = new List<Settings>();
        }
    }

    public class Settings
    {
        public Settings()
        {
            Name = "no name";

            Top = 250;
            Left = 125;

            DotDiameter = 6;
            DotColorA = 255;
            DotColorR = 250;
            DotColorG = 10;
            DotColorB = 10;
            DotVisibility = true;

            AngleThickness = 4;
            AngleLength = 30;
            AngleAngle = 45;//35~70
            AngleColorA = 255;
            AngleColorR = 250;
            AngleColorG = 10;
            AngleColorB = 10;
            AngleVisibility = true;

            CrossThickness = 3;
            CrossLength = 15;
            CrossOffset = 10;
            CrossRotation = 0;
            CrossColorA = 255;
            CrossColorR = 250;
            CrossColorG = 10;
            CrossColorB = 10;
            CrossVisibility = true; 
        }

        public string Name { get; set; }

        public double Top { get; set; }
        public double Left { get; set; }

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
