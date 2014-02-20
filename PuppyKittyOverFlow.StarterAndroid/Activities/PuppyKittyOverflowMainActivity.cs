using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;
using PuppyKittyOverFlow.StarterAndroid;

namespace PuppyKittyOverflow.StarterAndroid
{

  public class PuppyKittyState : Java.Lang.Object
  {
    public string Image { get; set; }
    public bool SetDefault { get; set; }
    public Task CurrentTask { get; set; }
  }

  [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/ic_launcher", Theme = "@style/Theme", HardwareAccelerated = false)]
  public class PuppyKittyOverflowMainActivity : Activity, IAccelerometerListener
  {
    private ProgressBar progressBar;
    private ImageView imageView;
    private Button buttonPuppy;
    private Button buttonKitty;
    private AnimatedImageView imageViewAnimated;
    private PuppyKittyState state;
    private AccelerometerManager accelerometerManager;
    private Task currentTask;


    protected async override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);


      accelerometerManager = new AccelerometerManager(this, this);

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
      imageViewAnimated = FindViewById<AnimatedImageView>(Resource.Id.imageview_animal_animated);
      progressBar.Visibility = ViewStates.Invisible;
      imageView.Visibility = ViewStates.Gone;
      imageViewAnimated.Visibility = ViewStates.Gone;
			state = LastNonConfigurationInstance as PuppyKittyState;

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

    private async Task LoadImage(OverflowHelper.Animal animal)
    {

      progressBar.Visibility = ViewStates.Visible;
      progressBar.Indeterminate = true;
      imageView.Visibility = ViewStates.Gone;
      imageViewAnimated.Visibility = ViewStates.Gone;
      buttonKitty.Enabled = false;
      buttonPuppy.Enabled = false;

      state.SetDefault = true;
      state.Image = animal == OverflowHelper.Animal.Cat ? "cat" : "dog";
      var eventType = string.Empty;
      switch (animal)
      {
        case OverflowHelper.Animal.Cat:
          eventType = "cat";
          break;
        case OverflowHelper.Animal.Dog:
          eventType = "dog";
          break;
        case OverflowHelper.Animal.Otter:
          eventType = "otter";
          break;
      }
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
				imageView.SetImageResource (Android.Resource.Color.Transparent);
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
					imageView.SetImageResource (Android.Resource.Color.Transparent);
          imageView.Visibility = ViewStates.Visible;
        }
      }

      this.InvalidateOptionsMenu();
    }

		public override Object OnRetainNonConfigurationInstance ()
		{
			base.OnRetainNonConfigurationInstance ();
			return state;
		}


    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      if (item.ItemId == Resource.Id.action_about)
      {
				var builder = new AlertDialog.Builder(this);
				builder
					.SetTitle(Resource.String.about)
					.SetMessage(Resource.String.about_text)
					.SetPositiveButton(Resource.String.ok, delegate {

					});             

				AlertDialog alert = builder.Create();
				alert.Show();
      }
      return base.OnOptionsItemSelected(item);
    }

		Android.Widget.ShareActionProvider actionProvider;
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      this.MenuInflater.Inflate(Resource.Menu.main_menu, menu);

      var shareItem = menu.FindItem(Resource.Id.action_share);
			var test = shareItem.ActionProvider;
			actionProvider = test.JavaCast<Android.Widget.ShareActionProvider>();
      if (state.SetDefault)
        shareItem.SetVisible(false);

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

  

		protected override void OnPause()
    {
			base.OnPause();
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

