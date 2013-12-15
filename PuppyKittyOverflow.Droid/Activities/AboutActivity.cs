using Android.App;
using Android.OS;
using Google.Analytics.Tracking;
using Android.Support.V7.App;

namespace PuppyKittyOverflow.Droid.Activities
{
  [Activity(Label = "@string/about", Icon = "@drawable/ic_launcher", Theme = "@style/Theme")]
  public class AboutActivity : ActionBarActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      SetContentView(Resource.Layout.About);
      // Create your application here
    }

    protected override void OnStart()
    {
      base.OnStart();

      // Setup Google Analytics Easy Tracker
      EasyTracker.GetInstance(this).ActivityStart(this);
      GAServiceManager.Instance.SetLocalDispatchPeriod(60);
    }

    protected override void OnStop()
    {
      base.OnStop();

      // Stop Google Analytics Easy Tracker
      EasyTracker.GetInstance(this).ActivityStop(this);
    }

  }


}