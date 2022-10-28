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
using Windows.UI;

namespace OnScreenReticleXboxGameBar
{
    public class JsonParser
    {
        private SettingsList settingsList;
        private JsonSerializer serializer;
        private Windows.Storage.StorageFolder installedLocation;
        private AutoResetEvent autoResetEvent;

        public SettingsList SettingsList { get => settingsList; set => settingsList = value; }

        public JsonParser()
        {
            autoResetEvent = new AutoResetEvent(false);

            installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            serializer = new JsonSerializer();
            DeserializeSettings();
            autoResetEvent.WaitOne(500);
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

            autoResetEvent.Set();
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

            Top = 300;
            Left = 125;

            DotDiameter = 6;
            DotColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            DotVisibility = true;

            AngleThickness = 3;
            AngleLength = 13;
            AngleAngle = 50;//35~70
            AngleColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            AngleVisibility = true;

            CrossThickness = 3.5;
            CrossLength = 10;
            CrossOffset = 10;
            CrossRotation = 0;
            CrossColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            CrossVisibility = true; 
        }

        public string Name { get; set; }

        public double Top { get; set; }
        public double Left { get; set; }

        // Dot
        public double DotDiameter { get; set; }
        public Color DotColor { get; set; }
        public bool DotVisibility { get; set; }

        // Angle
        public double AngleThickness { get; set; }
        public double AngleLength { get; set; }
        public double AngleAngle { get; set; }
        public Color AngleColor { get; set; }
        public bool AngleVisibility { get; set; }

        // Cross
        public double CrossThickness { get; set; }
        public double CrossLength { get; set; }
        public double CrossOffset { get; set; }
        public double CrossRotation { get; set; }
        public Color CrossColor { get; set; }
        public bool CrossVisibility { get; set; }
    }
}
