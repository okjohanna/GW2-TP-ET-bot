using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cat_Bot.GuildWars2
{
    internal class TradingPost
    {
        // https://api.guildwars2.com/v2/commerce/listings?ids=19721
        // https://api.guildwars2.com/v2/items?ids=96978
        
        public static async Task<string> GetListingInfoAsync(int itemId)
        {
            string baseUrl = "https://api.guildwars2.com/v2/";
            //string listingUrl = baseUrl + itemId;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"commerce/prices/{itemId}");
                    response.EnsureSuccessStatusCode();

                    //Read content as string
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
        }

    }
}
