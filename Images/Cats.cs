using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Cat_Bot.Cats
{
    internal class RandomCats
    {
        public static async Task<string> GetRandomCatAsync()
        {
            string catURL = "http://127.0.0.1:8000/cat";
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(catURL);
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