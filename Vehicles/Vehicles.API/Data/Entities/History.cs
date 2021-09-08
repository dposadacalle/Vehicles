using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Vehicles.API.Data.Entities
{
    public class History
    {
        public int Id { get; set; }

        [Display(Name = "Vehículo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Vehicle Vehicle { get; set; }

        [JsonIgnore]
        [Display(Name = "Mecanico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

        [Display(Name = "kilometraje")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Mileage { get; set; }

        [Display(Name = "Observacion")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public ICollection<Detail> Details { get; set; }

        [Display(Name = "# Detalles")]
        public int DetailsCount => Details == null ? 0 : Details.Count;

        [Display(Name = "Total Mano de Obra")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalLabor => Details == null ? 0 : Details.Sum(x => x.LaborPrice);

        [Display(Name = "Total Repuestos")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalSpareParts => Details == null ? 0 : Details.Sum(x => x.SparePartsPrice);

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total => Details == null ? 0 : Details.Sum(x => x.TotalPrice);

    }
}
