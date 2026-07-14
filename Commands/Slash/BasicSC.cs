using Cat_Bot.Cats;
using Cat_Bot.GuildWars2;
using Cat_Bot.Images;
using Cat_Bot.Quotes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;

namespace Cat_Bot.Commands.Slash
{
    public class BasicSC : ApplicationCommandModule
    {
        [SlashCommand("quote", "Sends a random quote.")]
        public async Task SlashCommandQuote(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            string randomQuote = await RandomQuotes.GetQuoteAsync();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(randomQuote));
        }

        [SlashCommand("cat", "Sends a random cat picture.")]
        public async Task SlashCommandCat(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            DiscordEmbed randomCat = await RandomCats.GetRandomCatAsync(ctx);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(randomCat));
        }

        /*
         * WAIFU API IS DOWN INDEFINITELY! =( Need to replace these with another source
        [SlashCommand("waifu", "Sends a random waifu GIF.")]
        public async Task SlashCommandWaifu(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            string randomWaifu = await Waifus.GetWaifuAsync();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(randomWaifu));
        }

        [SlashCommand("hehe", "Sends a smug waifu.")]
        public async Task SlashCommandHehe(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            string randomSmugWaifu = await Waifus.GetSmugWaifuAsync();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(randomSmugWaifu));
        }
        */

        [SlashCommand("et", "Lists all the upcoming group events in Guild Wars 2 that I care about.")]
        public async Task SlashCommandEt(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            // Call the static method GetUpcomingEventsInfo and pass the EventService instance
            DiscordEmbed eventsEmbed = EventService.GetUpcomingEventsEmbed(filterNext30Minutes: false);

            // Send the events information back to the user
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(eventsEmbed));
        }

        [SlashCommand("et30", "Shows group events in Guild Wars 2 starting in the next 30 minutes.")]
        public async Task SlashCommandEt30(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            DiscordEmbed eventsEmbed = EventService.GetUpcomingEventsEmbed(filterNext30Minutes: true);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(eventsEmbed));
        }

        [SlashCommand("ping", "Test command")]
        public async Task SlashCommandPing(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pong!"));
        }

        [SlashCommand("tp", "Shows the Trading Post price of an item by ID.")]
        public async Task SlashCommandTp(InteractionContext ctx,
            [Option("id", "The item ID (e.g., 19721 for Glob of Ectoplasm)")] long itemId)
        {
            await ctx.DeferAsync();

            var embed = await TradingPost.GetListingEmbedAsync((int)itemId);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
    }
}
