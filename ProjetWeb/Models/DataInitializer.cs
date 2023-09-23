using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace ProjetWeb.Models
{
    public static class DataInitializer
    {

        //C'est si drôle
        public static void SeedRole(RoleManager<IdentityRole> roleManager)
        {
            createRole(roleManager, "Admin");
            createRole(roleManager, "Driver");
            createRole(roleManager, "Customer");
            createRole(roleManager, "Dispatcher");
        }

        private static void createRole(RoleManager<IdentityRole> roleManager, string role)
        {

            if (roleManager.RoleExistsAsync(role).Result == false)
            {
                IdentityRole r = new IdentityRole() { Name = role };
                var result = roleManager.CreateAsync(r);
                result.Wait();
            }
        }

        public static async Task Seed(UserManager<User> userManager)
        {

            if (userManager.Users.Count() != 0) return;

            for (int i = 0; i < 5; i++)
            {
                var surName = new Bogus.Person().FirstName;
                var lastName = new Bogus.Person().LastName;
                var matricule = "E" + Randomizer.Seed.Next(100000, 1000000);
                
                var email = $"{surName}.{lastName}@HelmoBilite.be";
                var driver = new Driver
                {
                    UserName = email,
                    Name = surName + " " + lastName,
                    Email = email,
                    License = getLicense(),
                    Matricule = matricule,
                    
                };

                var result = userManager.CreateAsync(driver, "Mdp_123").Result;
               
                if(result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(driver, "Driver").Result;
                }

            }

            for (int i = 0; i < 5; i++)
            {

                var surName = new Bogus.Person().FirstName;
                var lastName = new Bogus.Person().LastName;
                var email = $"{surName}.{lastName}@HelmoBilite.be";
                var address = "Beyond the moon and stars";
                var customer = new Customer
                {
                    UserName = email,
                    Name = surName + " " + lastName,
                    Email = email,
                    Address = generateAdress(),
                    BadPayer = false,
                    
                };

                var result = userManager.CreateAsync(customer, "Mdp_123").Result;

                if (result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(customer, "Customer").Result;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                var surName = new Bogus.Person().FirstName;
                var lastName = new Bogus.Person().LastName;
                var matricule = "E" + Randomizer.Seed.Next(100000, 1000000);
                var email = $"{surName}.{lastName}@HelmoBilite.be";
                var dispatcher = new Dispatcher
                    {
                    UserName = email,
                    Name = surName + " " +lastName,
                    Email = email,  
                    StudyLvl = getStudy(),
                    Matricule = matricule,
                    
                };

                var result = userManager.CreateAsync(dispatcher, "Mdp_123").Result;

                if (result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(dispatcher, "Dispatcher").Result;
                }
            }

                var admin = new Admin
                {
                    UserName = "admin@admin.helmbolite.be",
                    Name = new Bogus.Person().FirstName + " " + new Bogus.Person().LastName,
                    Email = "stonks@HelmoBilite.be",
                    
                };

            var result_admin = userManager.CreateAsync(admin, "Mdp_123").Result;

            if (result_admin.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(admin, "Admin").Result;
            }


            /*for (int i = 0; i < 10; i++)
            {
                var order = new Order
                {
                    Id = i + 50,
                    StartDate = new DateTime(2023, i + 1, (i + 5) % 30, (i + 12) % 24, (i * 10) % 60, 0),
                    EndDate = new DateTime(2024, i + 1, (i + 5) % 30, (i + 12) % 24, (i * 10) % 60, 0),
                    Source = generateAdress(),
                    Destination = generateAdress(),
                    Content = getContains(i),
                    Truck = null
                };
            }

            for (int i = 0; i < 10; i++)
            {
                var truck = new Truck
                {
                    Id = i + 10,
                    Brand = "Iveco",
                    Model = "Eurocargo",
                    Plate = "1-ABC-5" + i,
                    Types = getLicense(),
                    MaxWeight = 8000
                };
            }*/

        }

        private static string getRandomLetter()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 3)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string getContains(int index)
        {
            string[] contains = { "20 tonnes de poisson", "un chat", "10 tonnes de bois", "des tables et chaises pour helmo", "une bombe nucléaire" };
            return contains[index % 5];
        }

        private static string getLicense()
        {
            Random random = new Random();
            return random.Next(2) % 2 == 0 ? "C" : "CE";
        }

        private static string generateAdress()
        {
            Random random = new Random();
            string[] nameStreet = { "Nuit", "Kleetlaan", "Pegasuslaan", "Help", "Croix", "Stonks", "Take my Money" };
            int num = Randomizer.Seed.Next(5, 125);
            string[] localite = { "Anvers", "Mons", "Liège", "Charleroi", "Ans" };
            int postalCode = Randomizer.Seed.Next(4000, 4699);
            return $"rue de {nameStreet[random.Next(7)]}, {num} {postalCode} {localite[random.Next(5)]}";
        }

        private static string getStudy()
        {
            Random random = new Random();
            string[] study = { "CESS", "Bachelier", "Licencier" };
            return study[random.Next(3)];
        }
       
    }
}
