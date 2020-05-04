using System.ComponentModel.DataAnnotations;
using System.Configuration; //Para usar listas
using System.Data.SqlClient; // Para usar Listas
using System.Collections.Generic; //Para usar Listas
using System.Web.Mvc; // Para usar SelectListItem

namespace Propio.Models
{
    public class ArticuloModel
    {
        
        public int Id { get; set; }

        [Display(Name = "Articulo")]

        [Required(ErrorMessage = "Title is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Topic is required.")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "Abstract is required.")]
        public string Abstract { get; set; }

        [Required(ErrorMessage = "PublishDate is required.")]
        public string PublishDate { get; set; }

        [Required(ErrorMessage = "Route is required.")]
        public string Route { get; set; }
        public List<SelectListItem> TopicsList { get; set; }


    }
}