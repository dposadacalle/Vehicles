using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.API.Data.Entities;

namespace Vehicles.API.Models
{
    public class VehicleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de vehiculo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de vehiculo.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int VehicleTypeId { get; set; } 

        public IEnumerable<SelectListItem> VehicleTypes { get; set; }

        [Display(Name = "Márca")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una márca.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int BrandId { get; set; } 

        public IEnumerable<SelectListItem> Brands { get; set; } 
         
        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1900, 3000, ErrorMessage = "Valor de módelo no válido.")]
        public int Model { get; set; }

        [Display(Name = "Placa")]
        [RegularExpression(@"[a-zA-Z]{3}[0-9]{2}[a-zA-Z0-9]", ErrorMessage = "Formato de placa incorrecto.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener {1} carácteres.")]
        public string Plaque { get; set; }

        [Display(Name = "Línea")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Line { get; set; }

        [Display(Name = "Color")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Color { get; set; }

        public string UserId { get; set; } 

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }

        public ICollection<VehiclePhoto> VehiclePhotos { get; set; }

        [Display(Name = "# Fotos")]
        public int VehiclePhotosCount => VehiclePhotos == null ? 0 : VehiclePhotos.Count;

        [Display(Name = "Foto")]
        public string ImageFullPath => VehiclePhotos == null || VehiclePhotos.Count == 0
            ? $"https://localhost:44345/images/noimage.png"
            : VehiclePhotos.FirstOrDefault().ImageFullPath;

        public ICollection<History> Histories { get; set; }

        [Display(Name = "# Historias")]
        public int HistoriesCount => Histories == null ? 0 : Histories.Count;
    }
}
