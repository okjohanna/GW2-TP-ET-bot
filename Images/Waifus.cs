using Cat_Bot.Cats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cat_Bot.Images
{
    public class Waifus
    {
        public static string[] sfwCategories = new string[]
        {
            "waifu",
            "neko",
            "shinobu",
            "megumin",
            "bully",
            "cuddle",
            "cry",
            "hug",
            "awoo",
            "kiss",
            "lick",
            "pat",
            "smug",
            "bonk",
            "yeet",
            "blush",
            "smile",
            "wave",
            "highfive",
            "handhold",
            "nom",
            "bite",
            "glomp",
            "slap",
            "kill",
            "kick",
            "happy",
            "wink",
            "poke",
            "dance",
            "cringe"
        };

        public static Random _random = new Random();
        public static string GetRandomWaifuCategory()
        {
            int index = _random.Next(sfwCategories.Length);
            string waifuCategory = Waifus.sfwCategories[index];
            return waifuCategory;
        }

        public static async Task<string> GetWaifuAsync()
        {
            string baseURL = "https://api.waifu.pics/sfw/";
            string waifuURL = baseURL + GetRandomWaifuCategory();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(waifuURL);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a dynamic object
                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    // Extract the URL of the image from the response
                    return result.url;

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return null;
                }

            }
        }

        public static async Task<string> GetSmugWaifuAsync()
        {
            string smugWaifuURL = "https://api.waifu.pics/sfw/smug";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(smugWaifuURL);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    return result.url;

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return null;
                }
            }
        }
    }
}
