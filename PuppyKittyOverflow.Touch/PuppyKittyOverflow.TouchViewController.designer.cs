// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PuppyKittyOverflow.Touch
{
	[Register ("PuppyKittyOverflow_TouchViewController")]
	partial class PuppyKittyOverflow_TouchViewController
	{
		[Outlet]
		UIKit.UIButton ButtonKitty { get; set; }

		[Outlet]
		UIKit.UIButton ButtonPuppy { get; set; }

		[Outlet]
		UIKit.UIImageView ImageViewAnimal { get; set; }

		[Outlet]
		UIKit.UIView ViewBackground { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonRandom { get; set; }

		[Action ("ButtonKitty_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonKitty_TouchUpInside (UIButton sender);

		[Action ("ButtonPuppy_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonPuppy_TouchUpInside (UIButton sender);

		[Action ("ButtonRandom_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonRandom_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonRandom != null) {
				ButtonRandom.Dispose ();
				ButtonRandom = null;
			}
		}
	}
}
