using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.IO;
using Java.IO;

namespace PuppyKittyOverflow.Droid.Helpers
{
    public class AnimatedImageView : View
    {


        private Movie mMovie;
        private long mMovieStart;

        public AnimatedImageView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
        }

        public AnimatedImageView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
        }

        public async Task Initialize(System.IO.Stream input)
        {
            Focusable = true;
            
            
            mMovie = Movie.DecodeStream(input);
            mMovieStart = 0;
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(Color.Transparent);           
            
            long now = Android.OS.SystemClock.UptimeMillis();
            if (mMovieStart == 0) {   // first time
                mMovieStart = now;
            }
            if (mMovie != null) {
                int dur = mMovie.Duration();
                if (dur == 0) {
                    dur = 1000;
                }
                var relTime = (int)((now - mMovieStart) % dur);
                mMovie.SetTime(relTime);
                mMovie.Draw(canvas, Width - mMovie.Width(),
                            Height - mMovie.Height());
                Invalidate();
            }
        }
    }
}