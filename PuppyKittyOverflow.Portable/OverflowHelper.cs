using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PuppyKittyOverflow.Portable
{

    

    public static class OverflowHelper
    {
        public enum Animal
        {
            Cat,
            Dog
        }

        private const string CatUrl = "http://catoverflow.com/api/query?limit=1&order=random";
        private const string DogUrl = "http://dogoverflow.com/api/query?limit=1&order=random";
        public async static Task<String> GetPictureAsync(Animal animal)
        {
            try
            {
                var url = animal == Animal.Cat ? CatUrl : DogUrl;

                var client = new HttpClient();
                if (client.DefaultRequestHeaders.CacheControl == null)
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.Timeout = new TimeSpan(0,0,10);
                
                return await client.GetStringAsync(url);
                

            }
            catch (Exception ex)
            {
                return string.Empty;

            }
            
        }
    }
}
