namespace SoftUniBlog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SoftUniBlog.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                // If the database is empty, populate sample data in it

                CreateUser(context, "admin@gmail.com", "123", "System Administrator");
                CreateUser(context, "pesho@gmail.com", "123", "Peter Ivanov");
                CreateUser(context, "merry@gmail.com", "123", "Maria Petrova");
                CreateUser(context, "geshu@gmail.com", "123", "George Petrov");

                CreateRole(context, "Administrators");
                AddUserToRole(context, "admin@gmail.com", "Administrators");

                CreatePost(context,
                    title: "Size vs. Strength",
                    body: @"A ton of factors influence strength beyond muscle size and skill with the movements used to test strength.  The strength of individual muscle fibers, normalized muscle force, muscle moment arms, and body proportions can all have significant, independent effects on strength.
Just as there’s massive variability in muscle growth – some people gaining a ton of muscle in response to training, and other people gaining very little – there’s massive variability in strength gains as well.  Normalized muscle force (how strong a muscle is relative to how large it is) can increase up to 39% for some people and decrease by as much as 5% for others, in response to the exact same training program.
Early on in training, there’s a very weak relationship between gains in muscle and gains in strength.  Gains in muscle mass may explain as little as 2% of the variation in strength gains for new lifters.
For more experienced lifters, gains in muscle mass may explain up to 65%+ of the variability in strength gains, highlighting hypertrophy as a key factor for strength gains in trained lifters.
Training style has a big impact on the ratio of strength you gain relative to size, with heavier training generally producing larger gains in strength.
",
                    date: new DateTime(2016, 03, 27, 17, 53, 48),
                    authorUsername: "merry@gmail.com"
                );

                CreatePost(context,
                    title: "How to Squat",
                    body: @"Are your feet stable, or do they move? (It’s not uncommon for people to start with their feet pointed too far forward, and for them to rotate out a bit during the initial descent; if the bottom of your shoes are too grippy for this to happen, then you may see the lateral part of the heel of your shoe raise a bit.)
Is your weight shifting excessively forward or backward?  Do your heels rise off the ground, or could you lift your toes up off the ground if you wanted to?
Does your spine flex at any point in the movement? (Lumbar is more common than thoracic; also note whether lumbar flexion actually occurs at the bottom of the squat, or whether your spine starts in hyperextension, and then just moves back toward neutral.)
",
                    date: new DateTime(2016, 05, 11, 08, 22, 03),
                    authorUsername: "merry@gmail.com"
                );
                CreatePost(context,
                    title: "Training Volume",
                    body: @"Studies across a variety of populations have demonstrated that muscles grow in a very broad variety of rep ranges. When training protocols are matched for number of sets, even with very different training volumes, they generally result in similar levels of muscle growth.
in strength and muscular endurance are still very much tied to the rep range used. At least when talking about hypertrophy-based training, it’s more useful to think of “training volume” as “total number of hard sets per muscle” than “sets x reps x load.”
Mr. Universe bodybuilding",
                    date: new DateTime(2016, 03, 27, 17, 53, 48),
                    authorUsername: "merry@gmail.com"
                );

                CreatePost(context,
                    title: "Amount Of Weight You Lift",
                    body: @"Comfortably lifting a weight that was once out of reach shows you how your body can adapt to overloads you place upon it. But reaching the next level of strength and size requires you to continue increasing the demands on your body, a concept known as progressive overload, which is a basic tenet of resistance training.",
                    date: new DateTime(2016, 02, 18, 22, 14, 38),
                    authorUsername: "pesho@gmail.com"
                );

                CreatePost(context,
                    title: "How Military Troops Stay Jacked Around The World!",
                    body: @"Today's servicemen and servicewomen are subject to deployments that may last as long as nine months. For some, this time is spent aboard a large military vessel out in the middle of the ocean or in a remote hole-in-the wall camp far from the creature comforts most of us take for granted.

But what they all share with one another is an expectation that simply being healthy isn't enough. Troops must maintain peak mental and physical readiness at all times. Lives literally depend on it.

For anyone who loves to challenge themselves in the gym or with physique-based goals, the demands are even tougher. This is how some of our country's bravest men and women get jacked no matter where the service takes them!

",
                    date: new DateTime(2016, 04, 11, 19, 02, 05),
                    authorUsername: "geshu@gmail.com"
                );

                CreatePost(context,
                    title: "Training Based On Muscle Fiber Type",
body: @"Most muscles in your body have a fairly even split of fast-twitch and slow-twitch muscle fibers; very few muscles are (on average) incredibly fast-twitch or slow-twitch dominant.
There’s not a practical test to know whether a particular muscle is composed primarily of fast-twitch or slow-twitch fibers.  The methods you’d typically use in a gym setting (seeing how many reps you’d get with a particular percentage of your 1rm) have virtually no predictive power.
The idea that you should train muscles differently based on their predominant muscle fiber type comes from the notion that fast-twitch muscle fibers respond best to heavy weights and low reps, and that slow-twitch muscle fibers respond best to light weights and high reps.  Evidence is still very mixed on this point – it’s not yet clear that particular training styles specifically target fast-twitch or slow-twitch fibers in the first place.
Even if there was good evidence for fiber type specific hypertrophy, and even if there was a good, practical test to know a muscle’s fiber type breakdown, it still wouldn’t change the general recommendation to keep training that muscle with a variety of rep ranges.
",
                    date: new DateTime(2016, 06, 30, 17, 36, 52),
                    authorUsername: "merry@gmail.com"
                );

                foreach (var categoryPath in Directory.GetDirectories(MapPath(@"~\Images\Equipment")))
                {
                    var category = categoryPath.Substring(categoryPath.LastIndexOf('\\') + 1);

                    foreach (var typePath in Directory.GetDirectories(categoryPath))
                    {
                        var type = typePath.Substring(typePath.LastIndexOf('\\') + 1);

                        var typeEntity = context.EquipmentTypes.FirstOrDefault(a => a.Name == type);
                        if (typeEntity == null)
                        {
                            var equipmentTypeName = type;

                            var imagePath = $@"/Images/Types/{type}.png";

                            AddEquipmentType(context, type, imagePath);
                        }


                        foreach (var equipmentFilename in Directory.GetFiles(typePath))
                        {
                            var name = Path.GetFileNameWithoutExtension(equipmentFilename);
                            var extension = Path.GetExtension(equipmentFilename);
                            var imagePath = $@"/Images/Equipment/{category}/{type}/{name}{extension}";

                            AddEquipment(context, name, imagePath, category, type);
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        private void AddEquipment(ApplicationDbContext context, string name, string imagePath, string category, string type)
        {
            var equipmentType = context.EquipmentTypes.First(a => a.Name == type);

            var equipment = new Equipment()
            {
                Name = name,
                ImagePath = imagePath,
                Type = equipmentType,
                Category = (Category)Enum.Parse(typeof(Category), category)
            };

            context.Equipments.Add(equipment);
        }

        private void AddEquipmentType(ApplicationDbContext context, string name, string imagePath)
        {
            var equipmentType = new EquipmentType()
            {
                Name = name,
                ImagePath = imagePath
            };

            context.EquipmentTypes.Add(equipmentType);
            context.SaveChanges();
        }

        private void CreateUser(ApplicationDbContext context,
            string email, string password, string fullName)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private void AddUserToRole(ApplicationDbContext context, string userName, string roleName)
        {
            var user = context.Users.First(u => u.UserName == userName);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var addAdminRoleResult = userManager.AddToRole(user.Id, roleName);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreatePost(ApplicationDbContext context,
            string title, string body, DateTime date, string authorUsername)
        {
            var post = new Post();
            post.Title = title;
            post.Body = body;
            post.Date = date;
            post.Author = context.Users.Where(u => u.UserName == authorUsername).FirstOrDefault();
            context.Posts.Add(post);
        }
        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }
    }


}

