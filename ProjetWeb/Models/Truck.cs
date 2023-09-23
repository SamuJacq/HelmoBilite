using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjetWeb.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ProjetWeb.Models
{
    public class Truck
    {
        public int? Id { get; set; }

        [MaxLength(200)]
        [Required]
        public String? Brand { get; set; }

        [MaxLength(200)]
        [Required]
        public String? Model { get; set; }

        [MaxLength(200)]
        [Required]
        public String? Plate { get; set; }

        [MaxLength(200)]
        [Required]
        public String? Types { get; set; }

        [Required]
        public int MaxWeight { get; set; }

        public ICollection<Order>? Orders { get; set; }


        public static List<SelectListItem> SetSelectTruck(DateTime StartDate, DateTime EndDate, DbSet<Order> Orders, DbSet<Truck> Trucks)
        {
            var truckSelect = Trucks
                .Where(t => !Orders
                        .Any(o => o.Truck.Id == t.Id && (o.StartDate <= StartDate && o.EndDate >= StartDate
                                    || (o.StartDate <= EndDate && o.EndDate >= EndDate))))
                .Select(truck => new SelectListItem { Text = truck.Plate + " License: " + truck.Types + " Max tonne: " + truck.MaxWeight, Value = truck.Id + "" })
                .ToList();
            return truckSelect;
        }

    }

}
