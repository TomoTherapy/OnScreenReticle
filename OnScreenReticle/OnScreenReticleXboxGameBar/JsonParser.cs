using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace OnScreenReticleXboxGameBar
{
    public class JsonParser
    {
        private SettingsList settingsList;
        private JsonSerializer serializer;
        private Windows.Storage.StorageFolder installedLocation;

        public SettingsList SettingsList { get => settingsList; set => settingsList = value; }

        public JsonParser()
        {
            installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            serializer = new JsonSerializer();
            DeserializeSettings();
        }

        public async void SerializeSettings()
        {
            // Create sample file; replace if exists.
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await storageFolder.CreateFileAsync("OnScreenReticle.json", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(jsonFile, JsonConvert.SerializeObject(settingsList));
        }

        public async void DeserializeSettings()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            if (File.Exists(Path.Combine(storageFolder.Path, "OnScreenReticle.json")))
            {
                StorageFile jsonFile = await storageFolder.GetFileAsync("OnScreenReticle.json");
                string json = await FileIO.ReadTextAsync(jsonFile);
                settingsList = JsonConvert.DeserializeObject<SettingsList>(json);
            }

            if (settingsList == null)
            {
                settingsList = new SettingsList() { ChosenOne = 0 };
                settingsList.List.Add(new Settings() { Name = "Model 1" });
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
