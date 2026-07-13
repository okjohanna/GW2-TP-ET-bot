using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cat_Bot.GuildWars2
{
    // Represent time-related details for an event
    public class EventTime
    {
        public int HourInitial { get; set; }
        public int HourMultiplier { get; set; }
        public int MinuteOffset { get; set; }
    }

    // Represent an event's details
    public class Event
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public EventTime Time { get; set; }
        public string Waypoint { get; set; }
    }

    // Static helper for event-related functionality
    public static class EventService
    {
        private static string _filePath = "GuildWars2/events.json";
        private static List<Event> _events;

        // Load events from a JSON file (Static method)
        static EventService()
        {
            LoadEvents();
        }

        private static void LoadEvents()
        {
            try
            {
                string jsonContent = File.ReadAllText(_filePath);
                _events = JsonConvert.DeserializeObject<List<Event>>(jsonContent) ?? new List<Event>();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: The events file was not found.");
                throw;
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: The events file contains invalid JSON.");
                throw;
            }
        }

        // Get the local current time based on the configured time zone
        private static DateTime GetLocalCurrentTime()
        {
            DateTime utcNow = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
        }
        
        // Calculate the next occurrence of an event based on the current time
        private static DateTime CalculateNextEventTime(Event eventItem, DateTime currentTime)
        {
            DateTime eventStart = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                                               eventItem.Time.HourInitial, eventItem.Time.MinuteOffset, 0, DateTimeKind.Unspecified);

            eventStart = TimeZoneInfo.ConvertTimeToUtc(eventStart, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));

            while (eventStart <= currentTime)
            {
                eventStart = eventStart.AddHours(eventItem.Time.HourMultiplier);
            }

            return eventStart;
        }

        // Embed test
        public static DiscordEmbed GetUpcomingEventsEmbed(bool filterNext30Minutes = false)
        {

            DateTime localCurrentTime = GetLocalCurrentTime();
            string timezoneAbbreviation = "UTC+02:00";  // Can be dynamically set
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Event timers",
                Color = DiscordColor.Orange,
                Description = $"Timers for Guild Wars 2 group events that I care about.\n" +
                              $"These are the real fun ones frfr :100::100:\n" +
                              $"All times are displayed in `{timezoneAbbreviation}`.\n\n" +
                              $"For the full event timer use the official Guild Wars 2 wiki:\n" +
                              $"https://wiki.guildwars2.com/wiki/Event_timers\n\n"                              
            };

            bool eventsFound = false;

            var eventList = new List<(Event eventItem, DateTime localEventStart)>(); 

            foreach (var eventItem in _events)
            {
                DateTime localEventStart = CalculateNextEventTime(eventItem, localCurrentTime);
                TimeSpan timeRemaining = localEventStart - localCurrentTime;

                // Check if event starts in less than 30 min
                if (!filterNext30Minutes || timeRemaining.TotalMinutes <= 30 && timeRemaining.TotalMinutes >= 0)
                {
                    eventList.Add((eventItem, localEventStart));
                }
            }

            // Sort events soonest to latest
            var sortedEvents = eventList.OrderBy(e => e.localEventStart).ToList();

            foreach (var (eventItem, localEventStart) in sortedEvents)
            {
                TimeSpan timeRemaining = localEventStart - localCurrentTime;
                embedBuilder.AddField(
                    $"\n\n{eventItem.Name}",
                    $"Next event starts at `{localEventStart:HH:mm}` in `{timeRemaining.Hours}h {timeRemaining.Minutes}m`\n" +
                    $"```{eventItem.Waypoint}```\n\n"
                );

                eventsFound = true;
            }

            if (filterNext30Minutes && !eventsFound)
            {
                embedBuilder.AddField(
                    $"\nNothing noteworthy is starting in the next 30 minutes.",
                    $"*Carry on.*"
                );
            }

            // Finalize and return the embed
            return embedBuilder.Build();
        }
    }
}
