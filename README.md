Puppy Kitty Overflow
===========

Written in C# with ([Xamarin](http://www.xamarin.com))  **Start Edition Compatible**

Open Source Project by ([@JamesMontemagno](http://www.twitter.com/jamesmontemagno)) 

Copyright 2013 ([Refractored LLC](http://www.refractored.com))

## Available for free on:
* ([Android](https://play.google.com/store/apps/details?id=com.refractored.puppykittyoverflow))
* ([iPhone](https://itunes.apple.com/us/app/puppy-kitty-overflow-random/id766177393?ls=1&mt=8))
* ([Windows Phone](http://www.windowsphone.com/en-us/store/app/puppy-kitty-overflow/95847bb0-a856-4d88-ad59-7e2e759e988c))
* 

## Why did I create this?
Puppy Kitty overflow was created originally as a Windows Phone application during a DVLUP weekend hackathon. We had 2 weeks to get an app published to the marketplace to be eligible for a free phone. This means you have about 2 days or so of development and hope that everything gets certified the very first time. This meant that the app had to be extremely simple and had low risk of crashes. Puppy Kitty Overflow was spawned and landed on the WP Marketplace with 3 days to spare! I started with a PCL because why not and I knew I wanted to bring it over to Android/iOS with Xamarin at some point, which they are now.

## How much code is shared?
To be honest not that much besides a few helper methods. A bulk amount of the work is done to display the animated images on each platform so I didn't put a lot of work into architecting MVVM stuff in. I knew it was a single page app so no real need to do to much there.

## What technology is used?
Everything is written in C# with Xamarin. I created my own custom views on Android and iOS for displaying GIF images as well as implemented my own shake detection on Android. I used a PCL with Profile 78 and used HTTP Client PCL library to use it cross platform. On Android I am using Google Analytics component & Support v7 AppCompat component. On iOS I am using Flurry Analytics that I bound and BTProgressHud from the component store. On Windows Phone I use MarkedUp and ImageTools. 


Many thanks to ([@abock](http://www.twitter.com/abock)) for creating ([catoverflow.com](http://www.catoverflow.com)) and ([dogoverflow.com](http://www.dogoverflow.com))

## License
Licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html)

## TODO
* Add Otters to WP
