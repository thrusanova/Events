namespace Events.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Seed initial data only if the database is empty
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminFullName = "System Administrator";
                var adminPassword = adminEmail;
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                CreateSeveralEvents(context);
            }
        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            // Create the "admin" user
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            // Create the "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add the "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreateSeveralEvents(ApplicationDbContext context)
        {
            context.Events.Add(new Event()
            {
                Title = "With book on the beach",
                ImageUrl = "http://znaci-bg.com/wp-content/uploads/2015/07/knigata-300x200.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(45).AddHours(21).AddMinutes(30)
            });

            context.Events.Add(new Event()
            {
                Title = "Spirit of Burgas",
                ImageUrl = "http://ru.content.eventim.com/static/uploaded/ru/lz07_200_200.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(-15).AddHours(23).AddMinutes(00),
                Comments = new HashSet<Comment>() {
                    new Comment() { Text = "Finally again!!", Author = context.Users.First() }
                }
            });

            context.Events.Add(new Event()
            {
                Title = "Folklore festival",
                ImageUrl = "http://images.ibox.bg/2007/08/30/ans%20zornica/430x349.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(22).AddHours(22).AddMinutes(15)
            });

            context.Events.Add(new Event()
            {
                Title = "Bourgas and the Sea",
                ImageUrl = "http://www.pepatabakova.com/Resource/storage/1/661/real_741f045b89031163a0673f0984f0a134eaa709e8.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(19).AddHours(10).AddMinutes(30),
                Duration = TimeSpan.FromHours(3.5),
                Comments = new HashSet<Comment>()
                {
                    new Comment() { Text = "Great competition!" },
                    new Comment() { Text = "The best music event for all time!", Author = context.Users.First() },

                }
            });

            context.Events.Add(new Event()
            {
                Title = "Sand Festival",
                ImageUrl = "http://orig06.deviantart.net/3907/f/2010/221/8/8/burgas_sand_fest_2010___12___by_berov.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(10).AddHours(18).AddMinutes(00),
                Duration = TimeSpan.FromHours(22),
            });

            context.Events.Add(new Event()
            {
                Title = "The big reading",
                ImageUrl = "http://13.interpres.org/wp-content/uploads/uprajnenia-za-ochite-chetene.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(-2).AddHours(12).AddMinutes(0),
                Author = context.Users.First(),
                Location="New flora center",
                Comments = new HashSet<Comment>() {
                    new Comment() { Text = "<Anonymous> comment" }
                }
            });

            context.Events.Add(new Event()
            {
                Title = "European football championship under 17",
                ImageUrl = "http://www.bfu.bg/uploads/posts/euro2015.jpg",
                StartDateTime = DateTime.Now.Date.AddDays(-28).AddHours(11).AddMinutes(30),
                Author = context.Users.First(),
                Description = "For the first time in Bulgaria only in the sea  city",
                Duration = TimeSpan.FromHours(2),
                Location = "Lazur Stadium (Burgas)",
                Comments = new HashSet<Comment>() {
                    new Comment() { Text = "Nice!" },
                    new Comment() { Text = "We'll be there.", Author = context.Users.First() },
                    new Comment() { Text = "All of us.", Author = context.Users.First() }
                }
            });

            context.SaveChanges();
        }
    }
}
