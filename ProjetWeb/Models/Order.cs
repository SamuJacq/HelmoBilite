using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjetWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ProjetWeb.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date de début")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Date de fin")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [MaxLength(200)]
        [Required]
        [Display(Name = "Contenu")]
        public String? Content { get; set; }

        public Truck? Truck { get; set; }

        [Required]
        [Display(Name = "Etat")]
        public Boolean Delivered { get; set; }
        [MaxLength(1000)]

        [Display(Name = "Commentaire")]
        public String? Comment { get; set; }

        public Boolean Accepted { get; set; }

        public Customer? Customer { get; set; }
        public Driver? Driver { get; set; }


        public static IEnumerable<Order> GetBy(StatFilterViewModel filter, DbSet<Order> Order, DbSet<Driver> Driver, DbSet<Customer> Customer)
        {

            IEnumerable<Order> orders = new List<Order>();

            switch (filter.Name)
            {
                case "Driver":
                    orders = GetByDriver(filter.NameDriver, Order, Driver).Result;
                    break;
                case "Date":
                    orders = GetByDate(filter.StartDate, filter.EndDate, Order).Result;
                    break;
                case "Customer":
                    orders = GetByCustomer(filter.NameCustomer, Order, Customer).Result;
                    break;
            }

            return orders;
        }

        public static IEnumerable<OrderListViewModel> GetAllOrder(DbSet<Order> Order, DbSet<Customer> Customer) {
            var orderListViewModel = Order
                .Join(Customer, o => o.Customer.Id, u => u.Id, (o, u) => new { Order = o, User = u })
                .Where(o => o.Order.Accepted == false && o.Order.Comment == null)
                .Select(o => new OrderListViewModel
                {
                    Id = o.Order.Id,
                    StartDate = o.Order.StartDate,
                    EndDate = o.Order.EndDate,
                    Source = o.Order.Source,
                    Destination = o.Order.Destination,
                    Content = o.Order.Content,
                    NameCustomer = o.User.Name,
                    BadPayer = o.User.BadPayer
                })
                .ToList();
            return orderListViewModel;
        }

        public static Order GetOrderById(int id, DbSet<Order> Orders)
        { 
            return Orders.FindAsync(id).Result;
        }

            public static DetailsOrderDispatcherViewModel GetOrderDetail(int id, DbSet<Order> Orders, DbSet<Customer> Customers)
        {
            var detailsOrderDispatcherViewModel = new DetailsOrderDispatcherViewModel();
            var InfoDetail =
                from Order in Orders
                join User in Customers on Order.Customer.Id equals User.Id
                where Order.Id == id
                select new
                {
                    User.Name,
                    User.Email,
                    User.Address,
                    User.BadPayer,
                    Order.Id,
                    Order.StartDate,
                    Order.EndDate,
                    Order.Source,
                    Order.Destination,
                    Order.Content,
                };
            detailsOrderDispatcherViewModel.NameCustomer = InfoDetail.FirstOrDefault().Name;
            detailsOrderDispatcherViewModel.EmailCustomer = InfoDetail.FirstOrDefault().Email;
            detailsOrderDispatcherViewModel.AddressCustomer = InfoDetail.FirstOrDefault().Address;
            detailsOrderDispatcherViewModel.BadPayer = InfoDetail.FirstOrDefault().BadPayer;
            detailsOrderDispatcherViewModel.IdOrder = InfoDetail.FirstOrDefault().Id;
            detailsOrderDispatcherViewModel.StartDate = InfoDetail.FirstOrDefault().StartDate;
            detailsOrderDispatcherViewModel.EndDate = InfoDetail.FirstOrDefault().EndDate;
            detailsOrderDispatcherViewModel.Source = InfoDetail.FirstOrDefault().Source;
            detailsOrderDispatcherViewModel.Destination = InfoDetail.FirstOrDefault().Destination;
            detailsOrderDispatcherViewModel.Content = InfoDetail.FirstOrDefault().Content;

            return detailsOrderDispatcherViewModel;
        }

        public static IEnumerable<Order> GetDateOrder(int id, DbSet<Order> Orders) {
            var order = Orders
                .Where(o => o.Id == id)
                .Select(o => new Order { StartDate = o.StartDate, EndDate = o.EndDate });
            return order;
        }

        private static async Task<IEnumerable<Order>> GetByDriver(string NameDriver, DbSet<Order> Order, DbSet<Driver> Driver)
        {

            var orders = await Order
                .Join(Driver, o => o.Driver.Id, d => d.Id, (o, d) => new { Order = o, Driver = d })
                .Where(d => d.Driver.Name == NameDriver)
                .Select(o => new Order
                {
                    Id = o.Order.Id,
                    StartDate = o.Order.StartDate,
                    EndDate = o.Order.EndDate,
                    Source = o.Order.Source,
                    Destination = o.Order.Destination,
                    Content = o.Order.Content,
                    Delivered = o.Order.Delivered,
                    Comment = o.Order.Comment
                })
                .ToListAsync();
            return orders;
        }

        public static void UpdateOrderAccept(Order order ,Truck Truck, Driver Driver, _DbContext context) {
            order.Truck = Truck;
            order.Driver = Driver;
            order.Accepted = true;
            context.Update(order);
            context.SaveChangesAsync();
        }

        public static async void UpdateOrderRemoveDriver(string id, DbSet<Order> Orders)
        {
            var listOrder = Orders
                .Where(o => o.Driver.Id == id)
                .ToList();

            ChangeOrder(listOrder, Orders);
        }

        public static async void UpdateOrderRemoveTruck(int id, DbSet<Order> Orders)
        {
            var listOrder = Orders
                .Where(o => o.Truck.Id == id)
                .ToList();

            ChangeOrder(listOrder, Orders);
        }

        private static void ChangeOrder(List<Order> listOrder, DbSet<Order> Orders) {
            foreach (var item in listOrder)
            {
                item.Accepted = false;
                item.Driver = null;
                item.Truck = null;
                Orders.Update(item);
            }
        }


        private  static async Task<IEnumerable<Order>> GetByCustomer(string NameCustomer, DbSet<Order> Order, DbSet<Customer> Customers)
        {
            var orders = await Order
                .Join(Customers, o => o.Customer.Id, c => c.Id, (o, c) => new { Order = o, Customer = c })
                .Where(d => d.Customer.Name == NameCustomer)
                .Select(o => new Order
                {
                    Id = o.Order.Id,
                    StartDate = o.Order.StartDate,
                    EndDate = o.Order.EndDate,
                    Source = o.Order.Source,
                    Destination = o.Order.Destination,
                    Content = o.Order.Content,
                    Delivered = o.Order.Delivered,
                    Comment = o.Order.Comment
                })
                .ToListAsync();
            return orders;
        }

        private static async Task<IEnumerable<Order>> GetByDate(DateTime StartDate, DateTime EndDate, DbSet<Order> order)
        {
            var orders = await order.Where(o => o.StartDate >= StartDate || o.EndDate <= EndDate).ToListAsync();
            return orders;
        }

    }
}
