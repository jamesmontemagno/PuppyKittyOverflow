#define STARTER
using System;
using CoreGraphics;
using BigTed;
using Foundation;
using UIKit;
using PuppyKittyOverflow.Portable;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace PuppyKittyOverflow.Touch
{
	public partial class PuppyKittyOverflow_TouchViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public PuppyKittyOverflow_TouchViewController (IntPtr handle) : base (handle)
		{
			Title = "Puppy Kitty Overflow";
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		
		private string image;
		private async void SetImage(OverflowHelper.Animal animal)
	    {
            ButtonKitty.Enabled = false;
            ButtonPuppy.Enabled = false;
   
            BTProgressHUD.Show("Finding adorable animals!"); //show spinner + text
            //start spinner

			image = await OverflowHelper.GetPictureAsync(animal);
	        bool loadDefault = true;
	        if (!string.IsNullOrWhiteSpace(image))
	        {

	            try
	            {
                    
                    var client = new HttpClient(new ModernHttpClient.NativeMessageHandler());
					var stream = await client.GetStreamAsync(image);
					var data = await GetDataAsync(stream);
					AnimatedImageView.GetAnimatedImageView(data, ImageViewAnimal);

                    
                    loadDefault = false;

	            }
	            catch (Exception ex)
	            {
					Console.WriteLine (ex);
	            }

	        }

	        if (loadDefault)
	        {
				ImageViewAnimal.Image = UIImage.FromBundle(animal == OverflowHelper.Animal.Cat ? "cat.png" : "dog.png");
	        }

	        ButtonKitty.Enabled = true;
	        ButtonPuppy.Enabled = true;
            BTProgressHUD.Dismiss();
            //stop spinner
	    }

		private async Task<NSData> GetDataAsync(Stream stream)
		{
			return await Task.Run (() =>  {return NSData.FromStream (stream);});
		}

		

		#region View lifecycle
		public void ApplicationWillResignActive(NSNotification notification)
		{
			try{
				ResignFirstResponder();
				ImageViewAnimal.StopAnimating();
			}
			catch(Exception ex) {
			}
		}

		public void ApplicationWillReturnActive(NSNotification notification)
		{
			try{
				BecomeFirstResponder();
				ImageViewAnimal.StartAnimating();
			}
			catch(Exception ex) {
			}
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

			if(!Application.IsiOS7)
		    	View.BackgroundColor = UIColor.LightGray;

            NSNotificationCenter.DefaultCenter.AddObserver (new NSString("UIApplicationWillResignActiveNotification"), ApplicationWillResignActive);
            NSNotificationCenter.DefaultCenter.AddObserver (new NSString("UIApplicationWillTerminateNotification"), ApplicationWillResignActive);
            NSNotificationCenter.DefaultCenter.AddObserver (new NSString("UIApplicationWillEnterForegroundNotification"), ApplicationWillReturnActive); 

		    ViewBackground.Layer.CornerRadius = 10.0f;

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Action, delegate {
				if(string.IsNullOrEmpty(image))
					return;

                Xamarin.Insights.Track("Shared");

				var shareText = "#PuppyKittyOverflow Adorable Animals: " + image;
				var social = new UIActivityViewController(new NSObject[] { new NSString(shareText)}, 
					new UIActivity[] { new UIActivity() });
				PresentViewController(social, true, null);
			});
		    // Perform any additional setup after loading the view, typically from a nib.
		}



		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			BecomeFirstResponder ();
		}

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			ResignFirstResponder ();
		}

		public override bool CanBecomeFirstResponder {
			get {
				return true;
			}
		}

		// Called after the iOS determines the motion wasn't noise (such as walking up stairs).
		public override void MotionEnded (UIEventSubtype motion, UIEvent evt)
		{
            base.MotionEnded(motion, evt);
			if (!ButtonKitty.Enabled)
				return;

			// if the motion was a shake
			if(motion == UIEventSubtype.MotionShake) {
				SetImage (OverflowHelper.Animal.Otter);
			}
		}



        partial void ButtonRandom_TouchUpInside(UIButton sender)
        {
            SetImage(OverflowHelper.Animal.Random);
        }

        partial void ButtonKitty_TouchUpInside(UIButton sender)
        {
            SetImage(OverflowHelper.Animal.Cat);
        }

        partial void ButtonPuppy_TouchUpInside(UIButton sender)
        {
            SetImage(OverflowHelper.Animal.Dog);
        }
		#endregion

	}
}

