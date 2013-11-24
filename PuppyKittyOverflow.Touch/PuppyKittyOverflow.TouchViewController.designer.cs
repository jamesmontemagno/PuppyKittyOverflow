// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace PuppyKittyOverflow.Touch
{
	[Register ("PuppyKittyOverflow_TouchViewController")]
	partial class PuppyKittyOverflow_TouchViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonKitty { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonPuppy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageViewAnimal { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewBackground { get; set; }

		[Action ("ButtonKittyClick:")]
		partial void ButtonKittyClick (MonoTouch.Foundation.NSObject sender);

		[Action ("ButtonPuppyClick:")]
		partial void ButtonPuppyClick (MonoTouch.Foundation.NSObject sender);

		[Action ("InfoButonClick:")]
		partial void InfoButonClick (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonKitty != null) {
				ButtonKitty.Dispose ();
				ButtonKitty = null;
			}

			if (ButtonPuppy != null) {
				ButtonPuppy.Dispose ();
				ButtonPuppy = null;
			}

			if (ImageViewAnimal != null) {
				ImageViewAnimal.Dispose ();
				ImageViewAnimal = null;
			}

			if (ViewBackground != null) {
				ViewBackground.Dispose ();
				ViewBackground = null;
			}
		}
	}
}
