using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PuppyKittyOverflow.Portable;
using PuppyKittyOverflow.Droid.Helpers;
using Object = Java.Lang.Object;
using AndroidHUD;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace PuppyKittyOverflow.Droid.Activities
{

    public class PuppyKittyState : Object
    {
        public string Image { get; set; }

        public bool SetDefault { get; set; }

        public Task CurrentTask { get; set; }
    }

    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/ic_launcher", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme", HardwareAccelerated = false)]
    public class PuppyKittyOverflowMainActivity : AppCompatActivity, IAccelerometerListener
    {
        ProgressBar progressBar;
        ImageView imageView;
        Button buttonPuppy;
        Button buttonKitty;
        Button buttonRandom;
        AnimatedImageView imageViewAnimated;
        PuppyKittyState state;
        AccelerometerManager accelerometerManager;


        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Insights.Initialize("6a6eb93e563f008f1e2e8d05de4b8d4c182f2321", this);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar (toolbar);

            accelerometerManager = new AccelerometerManager(this, this);

            buttonRandom = FindViewById<Button>(Resource.Id.button_random);
            buttonRandom.Click += async (sender, args) =>
            {
                state.CurrentTask = LoadImage(OverflowHelper.Animal.Random);
                await state.CurrentTask;
            };
            
            buttonPuppy = FindViewById<Button>(Resource.Id.button_puppy);
            buttonPuppy.Click += async (sender, args) =>
            {
                state.CurrentTask = LoadImage(OverflowHelper.Animal.Dog);
                await state.CurrentTask;
            };

            buttonKitty = FindViewById<Button>(Resource.Id.button_kitty);
            buttonKitty.Click += async (sender, args) => await LoadImage(OverflowHelper.Animal.Cat);

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressbar);
            imageView = FindViewById<ImageView>(Resource.Id.imageview_animal);
            imageViewAnimated = FindViewById<Helpers.AnimatedImageView>(Resource.Id.imageview_animal_animated);
            progressBar.Visibility = ViewStates.Invisible;
            imageView.Visibility = ViewStates.Gone;
            imageViewAnimated.Visibility = ViewStates.Gone;

            if (state != null)
            {
                if (state.CurrentTask == null || state.CurrentTask.IsCompleted)
                    await SetImage();
            }
            else
            {
                state = new PuppyKittyState();
                state.SetDefault = true;
            }

        }

        async Task LoadImage(OverflowHelper.Animal animal)
        {
            AndHUD.Shared.Show(this, "Loading adorable animals...", maskType: MaskType.Clear);
            progressBar.Visibility = ViewStates.Visible;
            progressBar.Indeterminate = true;
            imageView.Visibility = ViewStates.Gone;
            imageViewAnimated.Visibility = ViewStates.Gone;
            buttonKitty.Enabled = false;
            buttonPuppy.Enabled = false;

            state.SetDefault = true;
            state.Image = animal == OverflowHelper.Animal.Cat ? "cat" : "dog";
            try
            {
                var image =
                    await OverflowHelper.GetPictureAsync(animal);

                if (!string.IsNullOrWhiteSpace(image))
                {
                    state.SetDefault = false;
                    state.Image = image;
                }
            }
            catch (Exception)
            {
            }

            await SetImage();

            progressBar.Visibility = ViewStates.Invisible;
            progressBar.Indeterminate = false;
            buttonKitty.Enabled = true;
            buttonPuppy.Enabled = true;
            AndHUD.Shared.Dismiss(this);
        }

        async Task SetImage()
        {
            if (state == null)
                return;

            if (state.SetDefault)
            {
                imageView.SetImageResource(state.Image == "cat" ? Resource.Drawable.cat : Resource.Drawable.dog);
                imageView.Visibility = ViewStates.Visible;
            }
            else
            {
                try
                {
                    var stream = await OverflowHelper.GetStreamAsync(state.Image);
                    await imageViewAnimated.Initialize(stream);
                    imageViewAnimated.Visibility = ViewStates.Visible;
                }
                catch (Exception)
                {
                    imageView.SetImageResource(state.Image == "cat" ? Resource.Drawable.cat : Resource.Drawable.dog);
                    imageView.Visibility = ViewStates.Visible;
                }
            }

            this.InvalidateOptionsMenu();
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_about)
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder
					.SetTitle(Resource.String.about)
					.SetMessage(Resource.String.about_text)
					.SetPositiveButton("OK", delegate
                    {

                    });             

                var alert = builder.Create();
                alert.Show();
            }
            return base.OnOptionsItemSelected(item);
        }

        Android.Support.V7.Widget.ShareActionProvider actionProvider;
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.main_menu, menu);

            var shareItem = menu.FindItem(Resource.Id.action_share);
            var provider = MenuItemCompat.GetActionProvider(shareItem);
            actionProvider = provider.JavaCast<Android.Support.V7.Widget.ShareActionProvider>();

            if (state.SetDefault)
                shareItem.SetVisible(false);

            Xamarin.Insights.Track("Shared");

            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "#PuppyKittyOverflow Adorable Animals: " + (state.SetDefault ? string.Empty : state.Image));

            actionProvider.SetShareIntent(intent);


            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (accelerometerManager.IsSupported)
                accelerometerManager.StartListening();

            imageViewAnimated.Start();
        }
           

        protected override void OnStop()
        {
            base.OnStop();
            if (accelerometerManager.IsListening)
                accelerometerManager.StopListening();

            imageViewAnimated.Stop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (accelerometerManager.IsListening)
                accelerometerManager.StopListening();
        }

        public void OnAccelerationChanged(float x, float y, float z)
        {

        }

        public async void OnShake(float force)
        {
            if (!buttonKitty.Enabled)
                return;

            state.CurrentTask = LoadImage(OverflowHelper.Animal.Otter);
            await state.CurrentTask;
        }
    }
}

