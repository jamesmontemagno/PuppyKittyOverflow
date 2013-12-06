using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.IO;

namespace PuppyKittyOverflow.Droid.Helpers
{
  public class AnimatedImageView : View
  {


    private Movie movie;
    private long movieStart;

    public AnimatedImageView(Context context, IAttributeSet attrs) :
      base(context, attrs)
    {
    }

    public AnimatedImageView(Context context, IAttributeSet attrs, int defStyle) :
      base(context, attrs, defStyle)
    {
    }

    public async static Task<byte[]> ReadFully(Stream input)
    {
      return await Task.Run(() =>
      {
        var buffer = new byte[16*1024];
        using (var ms = new MemoryStream())
        {
          int read;
          while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
          {
            ms.Write(buffer, 0, read);
          }
          return ms.ToArray();
        }
      });
    }

    public async Task Initialize(System.IO.Stream input)
    {
      Focusable = true;

      try
      {
        if (false)
        {
          movie = Movie.DecodeStream(input);
          movieStart = 0;
        }
        else
        {
          var array = await ReadFully(input);
          movie = Movie.DecodeByteArray(array, 0, array.Length);
          var duration = movie.Duration();
        }
      }
      catch (Exception ex)
      {

      }

    }

    protected override void OnDraw(Canvas canvas)
    {
      canvas.DrawColor(Color.Transparent);
      Paint p = new Paint(); 
      p.AntiAlias = true; 
      SetLayerType(LayerType.Software, p);
      long now = Android.OS.SystemClock.UptimeMillis();
      if (movieStart == 0)
      {   // first time
        movieStart = now;
      }
      if (movie != null)
      {
        int dur = movie.Duration();
        if (dur == 0)
        {
          dur = 1000;
        }
        var relTime = (int)((now - movieStart) % dur);
        movie.SetTime(relTime);
        movie.Draw(canvas, (Width - movie.Width())/2.0f,
                    (Height - movie.Height())/2.0f);
        Invalidate();
      }
    }
  }
}