using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace OnScreenReticle
{
    class ViewModel : INotifyPropertyChanged
    {
        private double reticleSize;
        private double windowTop;
        private double windowLeft;
        private int colorR;
        private int colorG;
        private int colorB;
        private int colorA;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseEvent(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public double ReticleSize { get => reticleSize; set { reticleSize = Math.Round(value, 1); RaiseEvent(nameof(ReticleSize)); } }
        public double WindowTop { get => windowTop; set { windowTop = value; RaiseEvent(nameof(WindowTop)); } }
        public double WindowLeft { get => windowLeft; set { windowLeft = value; RaiseEvent(nameof(WindowLeft)); } }
        public int ColorR { get => colorR; set { colorR = value; RaiseEvent(nameof(ColorR)); RaiseEvent(nameof(ColorBrush)); } }
        public int ColorG { get => colorG; set { colorG = value; RaiseEvent(nameof(ColorG)); RaiseEvent(nameof(ColorBrush)); } }
        public int ColorB { get => colorB; set { colorB = value; RaiseEvent(nameof(ColorB)); RaiseEvent(nameof(ColorBrush)); } }
        public int ColorA { get => colorA; set { colorA = value; RaiseEvent(nameof(ColorA)); RaiseEvent(nameof(ColorBrush)); } }

        public Brush ColorBrush
        {
            get => new SolidColorBrush(Color.FromArgb(byte.Parse(ColorA.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorR.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorG.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorB.ToString(), NumberStyles.Integer)));
            set { return; }
        }

        public ViewModel()
        {
            ReticleSize = 6;
            ColorA = 255;
            ColorR = 255;
            ColorG = 50;
            ColorB = 50;
        }

        internal void Center_menuItem_Click()
        {
            WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
            WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.5 - 50;
        }

        internal void Hunt_menuItem_Click()
        {
            WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
            WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.6 - 50;
        }

        internal void UpAdjust_Click()
        {
            WindowTop = WindowTop - 1;
        }

        internal void RightAdjust_Click()
        {
            WindowLeft = WindowLeft + 1;
        }

        internal void DownAdjust_Click()
        {
            WindowTop = WindowTop + 1;
        }

        internal void LeftAdjust_Click()
        {
            WindowLeft = WindowLeft - 1;
        }
    }
}
