using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using PuppyKittyOverflow.Droid.Helpers;
using PuppyKittyOverflow.Portable;
using Object = Java.Lang.Object;
using System.Net.Http;

namespace PuppyKittyOverflow.Droid.Activities
{

  public class PuppyKittyState : Object
  {
    public string Image { get; set; }
    public bool SetDefault { get; set; }
  }

  [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/ic_launcher", Theme = "@style/Theme")]
  public class PuppyKittyOverflowMainActivity : Activity, IAccelerometerListener
  {
    private ProgressBar progressBar;
    private ImageView imageView;
    private Button buttonPuppy;
    private Button buttonKitty;
    private AnimatedImageView imageViewAnimated;
    private PuppyKittyState state;
    private AccelerometerManager accelerometerManager;

    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);


      accelerometerManager = new AccelerometerManager(this, this);

      buttonPuppy = FindViewById<Button>(Resource.Id.button_puppy);
      buttonPuppy.Click += (sender, args) => LoadImage(OverflowHelper.Animal.Dog);

      buttonKitty = FindViewById<Button>(Resource.Id.button_kitty);
      buttonKitty.Click += (sender, args) => LoadImage(OverflowHelper.Animal.Cat);

      progressBar = FindViewById<ProgressBar>(Resource.Id.progressbar);
      imageView = FindViewById<ImageView>(Resource.Id.imageview_animal);
      imageViewAnimated = FindViewById<Helpers.AnimatedImageView>(Resource.Id.imageview_animal_animated);
      progressBar.Visibility = ViewStates.Invisible;
      imageView.Visibility = ViewStates.Gone;
      imageViewAnimated.Visibility = ViewStates.Gone;
      state = LastNonConfigurationInstance as PuppyKittyState;

      if (state != null)
      {
        SetImage();
      }
      else
      {
        state = new PuppyKittyState();
      }

    }

    private async void LoadImage(OverflowHelper.Animal animal)
    {
      
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
    }

    private async Task SetImage()
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

          var httpClient = new HttpClient();
          await imageViewAnimated.Initialize(await httpClient.GetStreamAsync(state.Image));
          imageViewAnimated.Visibility = ViewStates.Visible;
        }
        catch (Exception)
        {
          imageView.SetImageResource(state.Image == "cat" ? Resource.Drawable.cat : Resource.Drawable.dog);
          imageView.Visibility = ViewStates.Visible;
        }
      }
    }

    public override Object OnRetainNonConfigurationInstance()
    {
      base.OnRetainNonConfigurationInstance();
      return state;
    }

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      MenuInflater.Inflate(Resource.Menu.main_menu, menu);
      return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      if (item.ItemId == Resource.Id.action_about)
      {
        var intent = new Intent(this, typeof(AboutActivity));
        StartActivity(intent);
      }
      return base.OnOptionsItemSelected(item);
    }

    protected override void OnResume()
    {
      base.OnResume();
      if (accelerometerManager.IsSupported)
        accelerometerManager.StartListening();

    }

    protected override void OnStop()
    {
      base.OnStop();
      if (accelerometerManager.IsListening)
        accelerometerManager.StopListening();
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

    public void OnShake(float force)
    {
      if (!buttonKitty.Enabled)
        return;

      LoadImage(OverflowHelper.Animal.Otter);
    }
  }
}

