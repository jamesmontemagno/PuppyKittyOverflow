using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;
using Microsoft.Phone.Controls;
using PuppyKittyOverflow.Portable;

namespace PuppyKittyOverflow.Phone
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();
            ImageTools.IO.Decoders.AddDecoder<PngDecoder>();
        }

       
        private async void ButtonCat_OnClick(object sender, RoutedEventArgs e)
        {
            MarkedUp.AnalyticClient.SessionEvent("Cat");
            LoadImage(true);
        }

        private async void ButtonDog_OnClick(object sender, RoutedEventArgs e)
        {
            MarkedUp.AnalyticClient.SessionEvent("Dog");
           LoadImage(false);
        }
        private Uri _ImageSource;
        public Uri ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String str)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(str);
                PropertyChanged(this, e);
            }
        }

        private string image;
        private async void LoadImage(bool cat)
        {
            LoadingProgressBar.Visibility = Visibility.Visible;
            CatDogImage.Visibility = Visibility.Collapsed;
            CatDogImageFail.Visibility = Visibility.Collapsed;
            ButtonCat.IsEnabled = false;
            ButtonDog.IsEnabled = false;
            ButtonSetLockScreen.IsEnabled = false;
            image =
                await OverflowHelper.GetPictureAsync(cat ? OverflowHelper.Animal.Cat : OverflowHelper.Animal.Dog);
            this.DataContext = this;
            try
            {   
                if (string.IsNullOrWhiteSpace(image))
                {
                    image = cat ? @"Assets\Cat.png" : @"Assets\Dog.png";
                    CatDogImageFail.Source = new BitmapImage(new Uri(image, UriKind.Relative));
                    CatDogImageFail.Visibility = Visibility.Visible;
                }
                else
                {
                    ImageSource = new Uri(image, UriKind.Absolute);

                    CatDogImage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                image = cat ? @"Assets\Cat.png" : @"Assets\Dog.png";
                CatDogImageFail.Source = new BitmapImage(new Uri(image, UriKind.Relative));
                CatDogImageFail.Visibility = Visibility.Visible;
            }


            LoadingProgressBar.Visibility = Visibility.Collapsed;
            ButtonCat.IsEnabled = true;
            ButtonDog.IsEnabled = true;
            ButtonSetLockScreen.IsEnabled = true;
        }

        private void ButtonSetLockScreen_OnClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}