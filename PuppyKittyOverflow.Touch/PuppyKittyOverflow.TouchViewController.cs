using System;
using System.Drawing;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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

		partial void ButtonKittyClick (NSObject sender)
		{
			FlurryAnalytics.Flurry.LogEvent("Cat");
            SetImage(true);
		}

		partial void ButtonPuppyClick (NSObject sender)
		{
			FlurryAnalytics.Flurry.LogEvent("Dog");
            SetImage(false);
		}
		private string image;
	    private async void SetImage(bool cat)
	    {
            ButtonKitty.Enabled = false;
            ButtonPuppy.Enabled = false;
   
            BTProgressHUD.Show("Finding adorable animals!"); //show spinner + text
            //start spinner

            image = await OverflowHelper.GetPictureAsync(cat ? OverflowHelper.Animal.Cat : OverflowHelper.Animal.Dog);
	        bool loadDefault = true;
	        if (!string.IsNullOrWhiteSpace(image))
	        {

	            try
	            {
                    
					var client = new HttpClient();
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
	            ImageViewAnimal.Image = UIImage.FromBundle(cat ? "cat.png" : "dog.png");
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

		partial void InfoButonClick (NSObject sender)
		{
			var action = new UIAlertView("About", "Copyright 2013 Refractored LLC, @JamesMontemagno, Images provided by Catoverflow.com/Dogoverflow.com created by @abock", null, "OK", null);
			action.Show();
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			if(!Application.IsiOS7)
		    	View.BackgroundColor = UIColor.LightGray;

		    ViewBackground.Layer.CornerRadius = 10.0f;

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Action, delegate {
				if(string.IsNullOrEmpty(image))
					return;

				var shareText = "#PuppyKittyOverflow Adorable Animals: " + image;
				var social = new UIActivityViewController(new NSObject[] { new NSString(shareText)}, 
					new UIActivity[] { new UIActivity() });
				PresentViewController(social, true, null);
			});
		    // Perform any additional setup after loading the view, typically from a nib.
		}



		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}



		#endregion

	}
}

