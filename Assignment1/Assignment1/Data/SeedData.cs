using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;

namespace Assignment1.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var context = new ApplicationDbContext(
                services.GetRequiredService<DbContextOptions<ApplicationDbContext>>());


            var needCats = !context.Categories.Any();
            if (needCats)
            {
                context.Categories.AddRange(
                    new Category { Name = "concert" },
                    new Category { Name = "music" },
                    new Category { Name = "comedy" },
                    new Category { Name = "sports" }
                );
                context.SaveChanges();
            }


            var concert = context.Categories.FirstOrDefault(c => c.Name == "concert");
            var comedy = context.Categories.FirstOrDefault(c => c.Name == "comedy");
            var music = context.Categories.FirstOrDefault(c => c.Name == "music");
            if (concert == null || comedy == null || music == null) return;


            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Title = "Zach Bryan - World Tour",
                        Description = "Zach Bryan World Tour: Toronto, ON, at Molson Amplitheatre",
                        StartsAt = new DateTime(2026, 07, 29),
                        TicketPrice = 125.00M,
                        AvailableTickets = 250,
                        CategoryId = concert.CategoryId
                    },
                    new Event
                    {
                        Title = "Dave Chappelle - The Unfiltered Reflection Tour",
                        Description = "Dave Chappelle at Toronto, ON, Scotiabank Arena",
                        StartsAt = new DateTime(2026, 11, 11),
                        TicketPrice = 175.99M,
                        AvailableTickets = 400,
                        CategoryId = comedy.CategoryId
                    },
                    new Event
                    {
                        Title = "Lady Gaga - Mayhem Ball: World Tour",
                        Description = "Lady Gaga at Toronto, ON, Rogers Centre",
                        StartsAt = new DateTime(2026, 09, 10),
                        TicketPrice = 300.00M,
                        AvailableTickets = 250,
                        CategoryId = concert.CategoryId
                    },
                    new Event
                    {
                        Title = "VELD Music Festival",
                        Description = "Electronic Music Festival at Toronto, ON, Downsview Park",
                        StartsAt = new DateTime(2026, 08, 01),
                        TicketPrice = 75.00M,
                        AvailableTickets = 250,
                        CategoryId = music.CategoryId
                    },
                    new Event
                    {
                        Title = "Chris Brown: BREEZY BOWL World Tour",
                        Description = "Chris Brown at Toronto, ON, Rogers Stadium",
                        StartsAt = new DateTime(2026, 08, 19),
                        TicketPrice = 165.00M,
                        AvailableTickets = 250,
                        CategoryId = concert.CategoryId
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
