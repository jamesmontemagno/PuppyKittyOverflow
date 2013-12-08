using Android.App;
using Android.OS;
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
    }
}