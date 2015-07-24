using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PuppyKittyOverflow.Portable
{



    public static class OverflowHelper
    {
        public enum Animal
        {
            Cat,
            Dog,
            Otter,
            Random
        }

        public async static Task<Stream> GetStreamAsync(string url)
        {
            var httpClient = new HttpClient(new ModernHttpClient.NativeMessageHandler());
            return await httpClient.GetStreamAsync(url);
        }

        const string CatUrl = "http://catoverflow.com/api/query?limit=1&order=random";
        const string DogUrl = "http://dogoverflow.com/api/query?limit=1&order=random";
        const string OtterUrl = "http://otteroverflow.com/api/query?limit=1&order=random";

        private static Random random = new Random();

        public async static Task<String> GetPictureAsync(Animal animal)
        {
            try
            {
                Xamarin.Insights.Track("GetPicture", new Dictionary<string, string>
                    {
                        {"animal", animal.ToString()}
                    });

                if(animal == Animal.Random)
                {
                    var next = random.Next(0, 100);
                    if(next < 35)
                        animal = Animal.Cat;
                    else if(next < 70)
                        animal = Animal.Dog;
                    else
                        animal = Animal.Otter;
                }
                
                
                var url = string.Empty;
                switch (animal)
                {
                    case Animal.Cat:
                        url = CatUrl;
                        break;
                    case Animal.Dog:
                        url = DogUrl;
                        break;
                    default:
                        url = OtterUrl;
                        break;
                }

                var client = new HttpClient(new ModernHttpClient.NativeMessageHandler());
                if (client.DefaultRequestHeaders.CacheControl == null)
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                client.DefaultRequestHeaders.CacheControl.NoStore = true;
                client.Timeout = new TimeSpan(0, 0, 10);

                var imageUrl = await client.GetStringAsync(url);
                if (!string.IsNullOrWhiteSpace(imageUrl) && imageUrl.EndsWith("\n"))
                    imageUrl = imageUrl.TrimEnd('\n');

                return imageUrl;


            }
            catch (Exception ex)
            {
                return string.Empty;

            }

        }
    }
}
