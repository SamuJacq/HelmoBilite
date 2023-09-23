using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ProjetWeb.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjetWeb.Models
{
    public class User : IdentityUser
    {        
        [Required] [MaxLength(200)]
        public String? Name { get; set; }

        [MaxLength(200)]

        public String? Matricule { get; set; }

        [MaxLength(200)]
        public DateTime? Birthday { get; set; }

        public String? Photo { get; set; }


        public override Boolean Equals(Object o)
        {
            if (!(o is User) || o == null) return false;

            return ((User)o).Id.Equals(Id);
        }

        public static async Task<User> GetbyId(_DbContext context, string id)
        {
            return await context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }
    }

    public class Dispatcher : User
    {
        public String? StudyLvl { get; set; }
        
    }

    public class Driver : User
    {
        public String? License { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public static IEnumerable<Order> GetOrders(DbSet<Order> DbOrders, string id, string state)
        {

            IEnumerable<Order> orders = new List<Order>();

            switch (state)
            {
                case "delivered":
                    orders = Driver.GetDeliveredOrders(DbOrders, id).Result;
                    break;
                case "undelivered":
                    orders = Driver.GetUndeliveredOrders(DbOrders, id).Result;
                    break;
                case "pending":
                    orders = Driver.GetInPogressOrders(DbOrders, id).Result;
                    break;
                default:
                    orders = GetAllOrders(DbOrders, id).Result;
                    break;
            }
            
            return orders;
        }

        public static List<SelectListItem> SetSelectDriver(DateTime StartDate, DateTime EndDate, DbSet<Order> Orders, DbSet<Driver> Drivers)
        {
            var driverSelect = Drivers
                    .Where(d => !Orders
                        .Any(o => o.Driver.Id == d.Id && (o.StartDate <= StartDate && o.EndDate >= StartDate
                                    || (o.StartDate <= EndDate.AddHours(1) && o.EndDate >= EndDate.AddHours(1)))))
                    .Select(driver => new SelectListItem { Text = driver.Name + " License: " + driver.License, Value = driver.Id })
                    .ToList();
            return driverSelect;
        }

        public static IEnumerable<ListDriverViewModel> GetAllDrivers(DbSet<Driver> Drivers)
        {
            var listDriver =
                from Driver in Drivers
                select new ListDriverViewModel
                {
                    Id = Driver.Id,
                    Name = Driver.Name,
                    Matricule = Driver.Matricule,
                    Email = Driver.Email,
                    License = Driver.License,
                };
            return listDriver;
        }

        private async static Task<IEnumerable<Order>> GetAllOrders(DbSet<Order> DbOrders, string id)
        {
            if (id.Equals(""))
            {
                return await DbOrders.ToListAsync();
            } else
            {
                return await DbOrders.Where(o => o.Driver.Id == id).ToListAsync();

            }

            
        }

        private async static Task<IEnumerable<Order>> GetInPogressOrders(DbSet<Order> DbOrders, string id)
        {
            return await DbOrders
                .Where(o => o.Driver.Id == id && o.Comment == null)
                .OrderBy(o => o.StartDate)
                .ToListAsync();
        }

        private async static Task<IEnumerable<Order>> GetDeliveredOrders(DbSet<Order> DbOrders, string id)
        {
            return await DbOrders
                .Where(o => o.Driver.Id == id && o.Delivered)
                .OrderBy(o => o.StartDate)
                .ToListAsync();
        }

        private async static Task<IEnumerable<Order>> GetUndeliveredOrders(DbSet<Order> DbOrders, string id)
        {
            return await DbOrders
                .Where(o => o.Driver.Id == id && !o.Delivered && o.Comment != null)
                .OrderBy(o => o.StartDate)
                .ToListAsync();
        }
    }

    public class Customer : User
    {

        public string? Address { get; set; }

        [Display(Name="Mauvais payeur")]
        public Boolean BadPayer { get; set; }

        //public Address? Address { get; set; }

        //public int? AdresseId { get; set; }

        public ICollection<Order>? Orders { get; set; }
        

        public static async void TagAsBAdPayer(_DbContext context, string id)
        {
            Customer c = (Customer)GetbyId(context, id).Result;

            if (c.BadPayer){
                c.BadPayer = false;
            } else { 
                c.BadPayer = true;
            }
            
            context.SaveChanges();
        }

        public static async Task<IEnumerable<Customer>> GetActiveCustomer(_DbContext context)
        {
            var customers = await context.Customers
                .Join(context.Orders, c => c.Id, o => o.Customer.Id, (c, o) => new Customer
                {
                    Id = c.Id,
                    Email = c.Email,
                    Name = c.Name,
                    Address = c.Address,
                    BadPayer = c.BadPayer,
                    Orders = c.Orders
                })

                .Where(c => c.Orders != null)
                .Select(c => new Customer()
                {
                    Id = c.Id,
                    Email = c.Email,
                    Name = c.Name,
                    Address = c.Address,
                    BadPayer = c.BadPayer,
                    Orders = c.Orders
                })
                .ToListAsync();
            
            return customers.GroupBy(c => c.Id).Select(g => g.First());

        }
    }

    public class Admin : User {

        public static List<SelectListItem> SetSelectDriver(DbSet<Driver> Drivers)
        {
            var driverSelect = Drivers
                    .OrderBy(driver => driver.Name)
                    .Select(driver => new SelectListItem { Text = driver.Name, Value = driver.Name })
                    .ToList();
            return driverSelect;
        }

        public static List<SelectListItem> SetSelectCustomer(DbSet<Customer> Customers)
        {
            var driverSelect = Customers
                    .OrderBy(customer => customer.Name)
                    .Select(customer => new SelectListItem { Text = customer.Name, Value = customer.Name })
                    .ToList();
            return driverSelect;
        }

    }

}
