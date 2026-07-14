using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cat_Bot.Cats
{
    internal class RandomCats
    {
        public static string[] botPhrase = new string[]
        {
            "Here you go, {0}.",
            "I went to great lengths to fetch this for you, {0}.",
            "{0}! I found a good one!",
            "I hope you like this one, {0}.",
            "I swear I had a better one somewhere... but here you go, {0}.",
            "Haha! You're gonna like this one, {0}.",
            "There's more where that came from! Here you go, {0}.",
            "Oh, this is a good one, {0}!",
            "Not gonna lie, I'm getting a little tired, {0}.",
            "It is my duty to present you with only the best, {0}.",
            "Your wish is my command, {0}. Prepare thyself for this glorious cat!",
            "At your service, {0}, the vault of cats is but a command away",
            "Coming right up, {0}!",
            "Purrfection.",
            "You ask, {0}, and I deliver.",
            "Gotcha, {0}!",
            "Your wish is my command, {0}.",
            "Here it is, {0}, a true masterpiece!",
            "I ventured far for this one, {0}, and it’s totally worth it!",
            "I dug deep for this, {0}. I hope it’s worthy of your gaze!",
            "Haha! I swear this one's a winner, {0}!",
            "Brace yourself, {0}, this cat is going to steal your heart.",
            "Another gem for you, {0}—enjoy this one!",
            "I think this one’s my best find yet, {0}!",
            "You’re in for a treat, {0}.",
            "I've been waiting to show you this one, {0}!",
            "This is actually a picture of my cousin, {0}.",
            "Ding! Here you go, {0}.",
            "Hey, {0}, haven't I shown you this one already?",
            "I'd like to go home now, {0}."
        };

        public static Random _random = new Random();
        public static string GetRandomPhrase(InteractionContext ctx)
        {
            int index = _random.Next(botPhrase.Length);
            string randomPhrase = RandomCats.botPhrase[index];

            // Format the phrase with the user's mention
            return string.Format(randomPhrase, ctx.User.Mention);
        }

        public static async Task<DiscordEmbed> GetRandomCatAsync(InteractionContext ctx)
        {
            // URL of the local FastAPI server endpoint
            string catURL = "http://127.0.0.1:8000/cat";
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(catURL);
                    response.EnsureSuccessStatusCode();
                  
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    string imageUrl = result.url;

                    // Pass ctx to GetRandomPhrase() to get random phrase with the user's mention
                    string randomPhrase = GetRandomPhrase(ctx);

                    var embed = new DiscordEmbedBuilder()
                        .WithDescription(randomPhrase)
                        .WithImageUrl(imageUrl)
                        .WithColor(DiscordColor.Orange);

                    return embed.Build();
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