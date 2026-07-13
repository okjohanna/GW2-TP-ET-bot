using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cat_Bot.Quotes
{
    public class RandomQuotes
    {
        public static async Task<string> GetQuoteAsync()
        {
            // URL of the local FastAPI server endpoint
            string quoteURL = "http://127.0.0.1:8000/quote";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(quoteURL);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    return result.quote;
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

