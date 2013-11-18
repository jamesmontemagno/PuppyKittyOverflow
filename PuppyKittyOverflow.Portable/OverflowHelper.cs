using System;
using System.Net.Http;
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

                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }

            }
            catch (Exception ex)
            {
                return string.Empty;

            }
            
        }
    }
}
