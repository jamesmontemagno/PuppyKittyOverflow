using System;
using Android.App;
using Android.Runtime;

namespace PuppyKittyOverflow.Droid
{
	[Application(Title="@string/app_name", Icon = "@drawable/ic_launcher", HardwareAccelerated = true)]
	public class App : Application
	{
		public App(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer)
		{
		}
	}
}

