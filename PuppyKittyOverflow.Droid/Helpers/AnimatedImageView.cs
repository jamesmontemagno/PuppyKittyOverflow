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
        
          var array = await ReadFully(input);
          movie = Movie.DecodeByteArray(array, 0, array.Length);
        
      }
      catch (Exception)
      {

      }

    }

    private bool playing = true;
    public void Start()
    {
      playing = true;
      this.Invalidate();
    }

    public void Stop()
    {
      playing = false;
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
        var movieWidth = (float)movie.Width();
        var movieHeight = (float)movie.Height();
        var scale = 1.0f;
        if (movieWidth > movieHeight)
        {
          scale = this.Width/movieWidth;
          if (scale*movieHeight > Height)
            scale = Height/movieHeight;
        }
        else
        {
          scale = this.Height/movieHeight;
          if (scale*movieWidth > Width)
            scale = Height/movieWidth;
        }


        
        canvas.Scale(scale, scale);
        try
        {
          
          movie.Draw(canvas, 0, 0, p);
        } catch(Exception)
        {

        }

        if(playing)
          Invalidate();
      }
    }
  }
}