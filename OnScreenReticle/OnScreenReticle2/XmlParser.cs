using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OnScreenReticle2
{
    public class XmlParser
    {
        private string path = @"OnScreenReticle2_Settings.xml";
        private XmlSerializer SettingsSerializer;
        public Settings settings { get; set; }

        public XmlParser()
        {
            SettingsSerializer = new XmlSerializer(typeof(Settings));
        }

        public void SaveSettings()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    SettingsSerializer.Serialize(writer, settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        settings = SettingsSerializer.Deserialize(reader) as Settings;
                    }
                }
                else
                {
                    settings = new Settings();
                    DefaultSettings();
                }
            }
            catch
            {
                settings = new Settings();
                DefaultSettings();
            }
        }

        public void DefaultSettings()
        {
            settings.ColorR = 255;
            settings.ColorG = 50;
            settings.ColorB = 50;
            settings.ColorA = 255;
            settings.DotDiameter = 6;
            settings.AngleThickness = 3;
            settings.AngleLength = 8;
            settings.WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.5 - 50;
            settings.WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
            settings.DotVisibility = true;
            settings.AngleVisibility = false;
            settings.CrossVisibility = false;
        }

    }

    public class Settings
    {
        public double DotDiameter { get; set; }
        public double AngleThickness { get; set; }
        public double AngleLength { get; set; }
        public double CrossThickness { get; set; }
        public double CrossLength { get; set; }
        public double CrossOffset { get; set; }
        public double WindowTop { get; set; }
        public double WindowLeft { get; set; }
        public int ColorR { get; set; }
        public int ColorG { get; set; }
        public int ColorB { get; set; }
        public int ColorA { get; set; }
        public bool DotVisibility { get; set; }
        public bool AngleVisibility { get; set; }
        public bool CrossVisibility { get; set; }
    }
}
