using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private object obj = new object();

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
            autoResetEvent.WaitOne(1000);
        }

        public async void SerializeSettings()
        {
            try
            {
                // Create sample file; replace if exists.
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile jsonFile = await storageFolder.CreateFileAsync("OnScreenReticle.json", CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteTextAsync(jsonFile, JsonConvert.SerializeObject(settingsList));
            }
            catch (Exception)
            {
                
            }
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

            Thread.Sleep(60);
            autoResetEvent.Set();
        }
    }

    public class SettingsList
    {
        public int ChosenOne { get; set; }
        public ObservableCollection<Settings> List { get; set; }

        public SettingsList()
        {
            List = new ObservableCollection<Settings>();
        }
    }

    public class Settings : INotifyPropertyChanged
    {
        // General
        public string Name { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public Color ThemeColor { get; set; }

        // Dot
        public double DotDiameter { get; set; }
        public Color DotColor { get; set; }
        public bool DotVisibility { get; set; }
        public string DotVisibilityString { get => DotVisibility ? "Visible" : "Collapsed"; }

        // Angle
        public double AngleThickness { get; set; }
        public double AngleLength { get; set; }
        public double AngleAngle { get; set; }
        public Color AngleColor { get; set; }
        public bool AngleVisibility { get; set; }
        public string AngleVisibilityString { get => AngleVisibility ? "Visible" : "Collapsed"; }
        public string AnglePoints { get => $"0,0 {AngleLength},0 {AngleLength - AngleThickness * (90 - (double)AngleAngle) / 45 * (AngleAngle < 45 ? (1 + (45 - AngleAngle) * 0.02) : (1 + (45 - AngleAngle) * 0.008))},{AngleThickness} {AngleThickness - AngleThickness * (55 - (double)AngleAngle) / 45},{AngleThickness}"; }

        // Cross
        public double CrossThickness { get; set; }
        public double CrossLength { get; set; }
        public double CrossOffset { get; set; }
        public double CrossRotation { get; set; }
        public Color CrossColor { get; set; }
        public bool CrossVisibility { get; set; }
        public string CrossVisibilityString { get => CrossVisibility ? "Visible" : "Collapsed"; }
        public string CrossOffsetString { get => $"0,{CrossOffset},0,0"; }

        public Settings()
        {
            Name = "no name";
            Top = 300;
            Left = 125;
            ThemeColor = new Color() { A = 0xB2, R = 0, G = 0xFF, B = 0xBF };

            DotDiameter = 6;
            DotColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            DotVisibility = true;

            AngleThickness = 3;
            AngleLength = 13;
            AngleAngle = 50;
            AngleColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            AngleVisibility = true;

            CrossThickness = 3.5;
            CrossLength = 10;
            CrossOffset = 10;
            CrossRotation = 0;
            CrossColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            CrossVisibility = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void NotifyAllProperties()
        {
            NotifyPropertyChanged(nameof(DotDiameter));
            NotifyPropertyChanged(nameof(DotColor));
            NotifyPropertyChanged(nameof(DotVisibilityString));

            NotifyPropertyChanged(nameof(AngleAngle));
            NotifyPropertyChanged(nameof(AngleColor));
            NotifyPropertyChanged(nameof(AngleVisibilityString));
            NotifyPropertyChanged(nameof(AnglePoints));

            NotifyPropertyChanged(nameof(CrossThickness));
            NotifyPropertyChanged(nameof(CrossLength));
            NotifyPropertyChanged(nameof(CrossRotation));
            NotifyPropertyChanged(nameof(CrossColor));
            NotifyPropertyChanged(nameof(CrossVisibilityString));
            NotifyPropertyChanged(nameof(CrossOffsetString));
        }
    }
}
