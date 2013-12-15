using Android.App;
using Android.OS;
using Google.Analytics.Tracking;

namespace PuppyKittyOverflow.Droid.Activities
{
    [Activity(Label = "@string/about", Icon = "@drawable/ic_launcher", Theme = "@style/Theme")]
    public class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.About);
            // Create your application here
        }

		protected override void OnStart ()
		{
			base.OnStart ();

			// Setup Google Analytics Easy Tracker
			EasyTracker.GetInstance (this).ActivityStart (this);

			// By default, data is dispatched from the Google Analytics SDK for Android every 30 minutes.
			// You can override this by setting the dispatch period in seconds.
			GAServiceManager.Instance.SetLocalDispatchPeriod (30);
		}

		protected override void OnStop ()
		{
			base.OnStop ();

			// Stop Google Analytics Easy Tracker
			EasyTracker.GetInstance (this).ActivityStop (this);
		}

    }


}