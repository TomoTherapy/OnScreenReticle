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
        private SettingsList settingsList;
        private StorageFolder storageFolder;

        public SettingsList SettingsList { get => settingsList; set => settingsList = value; }

        public JsonParser()
        {
            storageFolder = ApplicationData.Current.LocalFolder;
            DeserializeSettings();

            if (settingsList == null)
            {
                settingsList = new SettingsList() { ChosenOne = 0 };
                settingsList.List.Add(new Settings() { Name = "no name" });
            }
        }

        public void SerializeSettings()
        {
            try
            {
                StorageFile jsonFile = storageFolder.CreateFileAsync("OnScreenReticle.json", CreationCollisionOption.ReplaceExisting).AsTask().Result;
                FileIO.WriteTextAsync(jsonFile, JsonConvert.SerializeObject(settingsList)).AsTask();
            }
            catch (Exception)
            {
                
            }
        }

        public void DeserializeSettings()
        {
            if (File.Exists(Path.Combine(storageFolder.Path, "OnScreenReticle.json")))
            {
                StorageFile jsonFile = storageFolder.GetFileAsync("OnScreenReticle.json").AsTask().Result;
                string json = FileIO.ReadTextAsync(jsonFile).AsTask().Result;
                settingsList = JsonConvert.DeserializeObject<SettingsList>(json);
            }

            if (settingsList == null)
            {
                settingsList = new SettingsList() { ChosenOne = 0 };
                settingsList.List.Add(new Settings() { Name = "Center dot", ChevronVisibility = false, CrossVisibility = false });
                settingsList.List.Add(new Settings() { Name = "Center dot,cross", ChevronVisibility = false });
                settingsList.List.Add(new Settings() { Name = "Center chevron", DotVisibility = false, CrossVisibility = false });
            }
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
        public double Top { get; set; }// 0~600
        public double Left { get; set; }// 0~250
        public Color ThemeColor { get; set; }

        // Dot
        public double DotDiameter { get; set; }// 1~30
        public Color DotColor { get; set; }
        public bool DotVisibility { get; set; }
        public string DotVisibilityString { get => DotVisibility ? "Visible" : "Collapsed"; }

        // Chevron
        public double ChevronThickness { get; set; }// 1~10
        public double ChevronLength { get; set; }// 5~50
        public double ChevronAngle { get; set; }// 35~70
        public Color ChevronColor { get; set; }
        public bool ChevronVisibility { get; set; }
        public string ChevronVisibilityString { get => ChevronVisibility ? "Visible" : "Collapsed"; }
        public string ChevronPoints { get => $"0,0 {ChevronLength},0 {ChevronLength - ChevronThickness * (90 - (double)ChevronAngle) / 45 * (ChevronAngle < 45 ? (1 + (45 - ChevronAngle) * 0.02) : (1 + (45 - ChevronAngle) * 0.008))},{ChevronThickness} {ChevronThickness - ChevronThickness * (55 - (double)ChevronAngle) / 45},{ChevronThickness}"; }

        // Cross
        public double CrossThickness { get; set; }// 1~20
        public double CrossLength { get; set; }// 1~50
        public double CrossOffset { get; set; }// 0~30
        public double CrossRotation { get; set; }// 0~90
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
            DotColor = new Color() { A = 0xFF, R = 0xFA, G = 0xA, B = 0xA };
            DotVisibility = true;

            ChevronThickness = 3;
            ChevronLength = 13;
            ChevronAngle = 50;
            ChevronColor = new Color() { A = 0xFF, R = 0xFA, G = 0xA, B = 0xA };
            ChevronVisibility = true;

            CrossThickness = 3.5;
            CrossLength = 10;
            CrossOffset = 10;
            CrossRotation = 0;
            CrossColor = new Color() { A = 255, R = 0xFA, G = 0xA, B = 0xA };
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

            NotifyPropertyChanged(nameof(ChevronAngle));
            NotifyPropertyChanged(nameof(ChevronColor));
            NotifyPropertyChanged(nameof(ChevronVisibilityString));
            NotifyPropertyChanged(nameof(ChevronPoints));

            NotifyPropertyChanged(nameof(CrossThickness));
            NotifyPropertyChanged(nameof(CrossLength));
            NotifyPropertyChanged(nameof(CrossRotation));
            NotifyPropertyChanged(nameof(CrossColor));
            NotifyPropertyChanged(nameof(CrossVisibilityString));
            NotifyPropertyChanged(nameof(CrossOffsetString));
        }
    }
}
