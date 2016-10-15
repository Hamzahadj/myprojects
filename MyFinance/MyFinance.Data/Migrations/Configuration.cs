namespace MyFinance.Data.Migrations
{
    using MyFinance.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyFinance.Data.MyFinanceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MyFinance.Data.MyFinanceContext";
        }

        protected override void Seed(MyFinance.Data.MyFinanceContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
               context.Categories.AddOrUpdate(
                  p => p.Name,
                  new Category{ Name = "Medicament" },
                  new Category{ Name = "Meuble" },
                  new Category{ Name = "Vetement" }
                );
               context.SaveChanges();
        }
    }
}
