using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
    public class HorizontalAnimatedImage : INotifyPropertyChanged
    {
        private ImageSource _image;
        public ImageSource Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        private BitmapSource sourceImage;
        private int currentX;
        private int frameWidth;
        private int frameHeight;
        private DispatcherTimer timer;

        public HorizontalAnimatedImage(BitmapSource image, int width, int height, 
            TimeSpan interval)
        {
            this.sourceImage = image;
            this.frameWidth = width;
            this.frameHeight = height;
            this.timer = new DispatcherTimer();
            this.timer.Interval = interval;
            this.timer.Tick += (sender, e) => Animate();
            SetImage();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            currentX = 0;
            SetImage();
        }

        private void Animate()
        {
            currentX += frameWidth;
            if (currentX + frameWidth >= sourceImage.Width)
                currentX = 0;
            SetImage();
        }

        private void SetImage()
        {
            Image = new CroppedBitmap(sourceImage,
                new Int32Rect(currentX, 0, frameWidth, frameHeight));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
