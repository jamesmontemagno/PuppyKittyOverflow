using System;
using System.Drawing;
using BigTed;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PuppyKittyOverflow.Portable;
using SDWebImage;

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
            SetImage(true);
		}

		partial void ButtonPuppyClick (NSObject sender)
		{
            SetImage(false);
		}

	    private async void SetImage(bool cat)
	    {
            ButtonKitty.Enabled = false;
            ButtonPuppy.Enabled = false;
   
            BTProgressHUD.Show("Finding adorable animals!"); //show spinner + text
            //start spinner

            var image = await OverflowHelper.GetPictureAsync(cat ? OverflowHelper.Animal.Cat : OverflowHelper.Animal.Dog);
	        bool loadDefault = true;
	        if (!string.IsNullOrWhiteSpace(image))
	        {

	            try
	            {
                    
	                var imageView = AnimatedImageView.GetAnimatedImageView(image, ImageViewAnimal);

                    
                    loadDefault = false;

	            }
	            catch (Exception ex)
	            {
	                
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

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		    View.BackgroundColor = UIColor.LightGray;
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

