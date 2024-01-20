using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool IsProduction)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), IsProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool IsProduction)
        {
            if(IsProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations...");

                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migration: {ex.Message}");
                }
                
            }
            else
            {
                if(!context.PLatforms.Any())
                {
                    Console.WriteLine("--> Seeding Data...");

                    context.PLatforms.AddRange(
                        new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                        new Platform() {Name="SQL", Publisher="Microsoft", Cost="Free"},
                        new Platform() {Name="Kubernetes", Publisher="Cloud Native", Cost="Free"}
                    );

                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("--> we alredy have data");
                }
            }
        }
    }
}