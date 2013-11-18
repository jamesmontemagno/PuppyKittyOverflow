using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using PuppyKittyOverflow.Portable;

namespace PuppyKittyOverflow.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

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

        private string image;
        private async void LoadImage(bool cat)
        {
            LoadingProgressBar.Visibility = Visibility.Visible;
            CatDogImage.Visibility = Visibility.Collapsed;
            ButtonCat.IsEnabled = false;
            ButtonDog.IsEnabled = false;
            ButtonSetLockScreen.IsEnabled = false;
            image =
                await OverflowHelper.GetPictureAsync(cat ? OverflowHelper.Animal.Cat : OverflowHelper.Animal.Dog);

            try
            {

                if (string.IsNullOrWhiteSpace(image))
                {
                    image = cat ? @"Assets\Cat.png" : @"Assets\Dog.png";

                    CatDogImage.Source = new BitmapImage(new Uri(image, UriKind.Relative));
                }
                else
                {
                    CatDogImage.Source = new BitmapImage(new Uri(image, UriKind.Absolute));
                }
            }
            catch (Exception)
            {
                image = cat ? @"Assets\Cat.png" : @"Assets\Dog.png";

                CatDogImage.Source = new BitmapImage(new Uri(image, UriKind.Relative));
            }


            LoadingProgressBar.Visibility = Visibility.Collapsed;
            CatDogImage.Visibility = Visibility.Visible;
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