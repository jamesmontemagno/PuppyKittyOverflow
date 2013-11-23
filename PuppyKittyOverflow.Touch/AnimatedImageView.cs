using System.Collections.Generic;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.ImageIO;
using MonoTouch.UIKit;

namespace PuppyKittyOverflow.Touch
{
    public class AnimatedImageView
    {
        public static UIImageView GetAnimatedImageView(string url, UIImageView imageView = null)
        {
            var sourceRef = CGImageSource.FromUrl(NSUrl.FromString(url));
            return CreateAnimatedImageView(sourceRef, imageView);

            //var source = GetAnimatedImageView(NSData.FromUrl(NSUrl.FromString(url)));

        }

        public static UIImageView GetAnimatedImageView(NSData nsData, UIImageView imageView = null)
        {
            var sourceRef = CGImageSource.FromData(nsData);

            return CreateAnimatedImageView(sourceRef, imageView);
        }

        private static UIImageView CreateAnimatedImageView(CGImageSource imageSource, UIImageView imageView = null)
        {
            var frameCount = imageSource.ImageCount;

            var frameImages = new List<NSObject>(frameCount);
            var frameCGImages = new List<CGImage>(frameCount);
            var frameDurations = new List<double>(frameCount);

            var totalFrameDuration = 0.0;

            for (int i = 0; i < frameCount; i++)
            {
                var frameImage = imageSource.CreateImage(i, null);

                frameCGImages.Add(frameImage);
                frameImages.Add(NSObject.FromObject(frameImage));

                var properties = imageSource.GetProperties(i, null);
                var duration = properties.Dictionary["{GIF}"];
                var delayTime = duration.ValueForKey(new NSString("DelayTime"));
                var realDuration = double.Parse(delayTime.ToString());
                frameDurations.Add(realDuration);
                totalFrameDuration += realDuration;
            }

            var framePercentageDurations = new List<NSNumber>(frameCount);
            var framePercentageDurationsDouble = new List<double>(frameCount);
            NSNumber currentDurationPercentage = 0.0f;
            double currentDurationDouble = 0.0f;
            for (int i = 0; i < frameCount; i++)
            {
                if (i != 0)
                {
                    var previousDuration = frameDurations[i - 1];
                    var previousDurationPercentage = framePercentageDurationsDouble[i - 1];

                    var number = previousDurationPercentage + (previousDuration/totalFrameDuration);
                    currentDurationDouble = number;
                    currentDurationPercentage = new NSNumber(number);
                }
                framePercentageDurationsDouble.Add(currentDurationDouble);
                framePercentageDurations.Add(currentDurationPercentage);
            }

            var imageSourceProperties = imageSource.GetProperties(null);
            var imageSourceGIFProperties = imageSourceProperties.Dictionary["{GIF}"];
            var loopCount = imageSourceGIFProperties.ValueForKey(new NSString("LoopCount"));
            var imageSourceLoopCount = float.Parse(loopCount.ToString());
            var frameAnimation = new CAKeyFrameAnimation();
            frameAnimation.KeyPath = "contents";
            if (imageSourceLoopCount <= 0.0f)
            {
                frameAnimation.RepeatCount = float.MaxValue;
            }
            else
            {
                frameAnimation.RepeatCount = imageSourceLoopCount;
            }

            frameAnimation.CalculationMode = CAAnimation.AnimationDescrete;
            frameAnimation.Values = frameImages.ToArray();
            frameAnimation.Duration = totalFrameDuration;
            frameAnimation.KeyTimes = framePercentageDurations.ToArray();
            frameAnimation.RemovedOnCompletion = false;
            var firstFrame = frameCGImages[0];
            if(imageView == null)
                imageView = new UIImageView(new RectangleF(0.0f, 0.0f, firstFrame.Width, firstFrame.Height));
            else
                imageView.Layer.RemoveAllAnimations();

            imageView.Layer.AddAnimation(frameAnimation, "contents");


            return imageView;
        }
    }
}