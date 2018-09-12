namespace NSDBz.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NSDBz.DAL.ZoologyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NSDBz.DAL.ZoologyContext context)
        {
            context.Countries.AddOrUpdate(
                p => p.Name,
                new Country { Name = "New Zealand" },
                new Country { Name = "Australia" }
 
                );
            context.SaveChanges();

            context.Species.AddOrUpdate(
                p => p.Name,
                new Species { Name = "Cassowary" },
                new Species { Name = "Echidna" }
                
                );
            context.SaveChanges();
        }
    }
}
