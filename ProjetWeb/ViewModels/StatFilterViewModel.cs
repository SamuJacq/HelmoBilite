using ProjetWeb.Models;
using System.ComponentModel;

namespace ProjetWeb.ViewModels
{
    public class StatFilterViewModel
    {

        public string Name { get; set; }

        [DisplayName("nom du chauffeur")]
        public string? NameDriver { get; set; }

        [DisplayName("nom du client")]
        public string? NameCustomer { get; set; }

        [DisplayName("Date de chargement")]
        public DateTime StartDate { get; set; }

        [DisplayName("Date de déchargement")]
        public DateTime EndDate { get; set; }


    }
}
