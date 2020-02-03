namespace VITV_Web.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Utilities;
    using System.Linq;
    using VITV_Web.Models;
    class MyCodeGenerator : CSharpMigrationCodeGenerator
    {
        protected override void Generate(
            DropIndexOperation dropIndexOperation, IndentedTextWriter writer)
        {
            dropIndexOperation.Table = StripDbo(dropIndexOperation.Table);

            base.Generate(dropIndexOperation, writer);
        }

        // TODO: Override other Generate overloads that involve table names

        private string StripDbo(string table)
        {
            if (table.StartsWith("dbo."))
            {
                return table.Substring(4);
            }

            return table;
        }
    }
    internal sealed class Configuration : DbMigrationsConfiguration<VITV_Web.DAL.VITVContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            CodeGenerator = new MyCodeGenerator();
        }

        protected override void Seed(VITV_Web.DAL.VITVContext context)
        {
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "quantri"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "quantri" };

                manager.Create(user, "vitv123");
                manager.AddToRole(user.Id, "Admin");
            }
        }
    }

    public class CodeMigrator : CSharpMigrationCodeGenerator
    {
        protected override void Generate(
            DropIndexOperation dropIndexOperation, IndentedTextWriter writer)
        {
            dropIndexOperation.Name = StripDbo(dropIndexOperation.Name);
            base.Generate(dropIndexOperation, writer);
        }

        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation, IndentedTextWriter writer)
        {
            dropForeignKeyOperation.Name = StripDbo(dropForeignKeyOperation.Name);
            base.Generate(dropForeignKeyOperation, writer);
        }

        protected override void Generate(DropPrimaryKeyOperation dropPrimaryKeyOperation, IndentedTextWriter writer)
        {
            dropPrimaryKeyOperation.Name = StripDbo(dropPrimaryKeyOperation.Name);
            base.Generate(dropPrimaryKeyOperation, writer);
        }

        // TODO: Override other Generate overloads that involve table names

        private string StripDbo(string name)
        {
            return name.Replace("dbo.", "");
        }
    }
}
