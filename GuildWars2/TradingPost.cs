using DSharpPlus.Entities;
using Jint.Runtime.Descriptors;
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
        private static readonly HttpClient http = new HttpClient()
        {
            BaseAddress = new Uri("https://api.guildwars2.com/v2/")
        };

        // -------- Coin icons (DISCORD EMBED IS NOT ABLE TO DISPLAY THESE!!)
        /*
        public static class CoinIcons
        {
            public static string Gold {  get; private set; }
            public static string Silver { get; private set; }
            public static string Copper { get; private set; }
            private static bool _loaded = false;

            public static async Task InitializeAsync()
            {
                if (_loaded) return;

                try
                {
                    string url = "files?ids=ui_coin_gold,ui_coin_silver,ui_coin_copper";
                    string response = await http.GetStringAsync(url);
                    var files = JsonConvert.DeserializeObject<List<Gw2File>>(response);

                    Gold = files?.Find(f => f.Id == "ui_coin_gold")?.Icon;
                    Silver = files?.Find(f => f.Id == "ui_coin_silver")?.Icon;
                    Copper = files?.Find(f => f.Id == "ui_coin_copper")?.Icon;

                    _loaded = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
        }
        */

        // -------- Money Formatter
        public static class Gw2MoneyFormatter
        {
            public static string FormatWithIcons(int copper)
            {
                int gold = copper / 10000;
                int silver = (copper % 10000) / 100;
                int copperLeft = copper % 100;
                /*
                string goldIcon = CoinIcons.Gold ?? "";
                string silverIcon = CoinIcons.Silver ?? "";
                string copperIcon = CoinIcons.Copper ?? "";
                */

                // Always show coins in descending order, skip zero values except for copper
                var result = "";

                if (gold > 0)
                    result += $"{gold} <:gold_coin:1438800249589076110>";
                if (silver > 0)
                    result += $"{silver} <:silver_coin:1438800104738918420>";

                // Always show copper if it's non-zero OR nothing else was added
                if (copperLeft > 0 || (gold == 0 && silver == 0))
                    result += $"{copperLeft} <:copper_coin:1438800276411777144>";

                return result.Trim();
            }
        }

        // https://api.guildwars2.com/v2/files?ids=ui_coin_gold,ui_coin_silver,ui_coin_copper
        // https://api.guildwars2.com/v2/commerce/listings?ids=19721
        // https://api.guildwars2.com/v2/items?ids=96978

        // -------- Models
        private class Gw2File
        {
            public string Id { get; set; } = "";
            public string Icon { get; set; } = "";
        }

        private class Gw2Price
        {
            public int Id {  get; set; }
            public Gw2Offer Buys { get; set; } = new Gw2Offer();
            public Gw2Offer Sells { get; set; } = new Gw2Offer();
        }
        
        private class Gw2Offer
        {
            public int Quantity { get; set; }
            public int Unit_Price { get; set; }
        }

        private class  Gw2Item
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Icon { get; set; } = "";
        }

        // -------- Fetch listing & item info
        public static async Task<DiscordEmbed> GetListingEmbedAsync(int itemId)
        {
            var price = await GetPriceAsync(itemId);
            var item = await GetItemAsync(itemId);

            if (price == null || item == null)
            {
                Console.WriteLine($"Could not fetch data for item ID `{itemId}`.");
                return null;
            }

            string sell = Gw2MoneyFormatter.FormatWithIcons(price.Sells.Unit_Price);
            string buy = Gw2MoneyFormatter.FormatWithIcons(price.Buys.Unit_Price);

            var embed = new DiscordEmbedBuilder()
                .WithTitle(item.Name)
                .WithThumbnail(item.Icon)
                .AddField("Sell Price", sell)
                .AddField("Buy Price", buy)
                .WithColor(DiscordColor.Orange);

            return embed.Build();
        }

        private static async Task<Gw2Price> GetPriceAsync(int itemId)
        {
            try
            {
                string result = await http.GetStringAsync($"commerce/prices/{itemId}");
                return JsonConvert.DeserializeObject<Gw2Price>(result);
            }
            catch
            {
                return null;
            }
        }

        private static async Task<Gw2Item> GetItemAsync(int itemId)
        {
            try
            {
                string json = await http.GetStringAsync($"items/{itemId}");
                return JsonConvert.DeserializeObject<Gw2Item>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}