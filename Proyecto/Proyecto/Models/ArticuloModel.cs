using System.ComponentModel.DataAnnotations;
using System.Configuration; //Para usar listas
using System.Data.SqlClient; // Para usar Listas
using System.Collections.Generic; //Para usar Listas
using System.Web.Mvc; // Para usar SelectListItem

namespace Proyecto.Models
{
    public class ArticuloModel
    {
        public int articleId { get; set; }

        [Display(Name = "Articulo")]

        [Required(ErrorMessage = "Title is required.")]
        public string name { get; set; }

        //[Required(ErrorMessage = "Topic is required.")]
        //public string Topic { get; set; }

        [Required(ErrorMessage = "Abstract is required.")]
        public string Abstract { get; set; }

        [Required(ErrorMessage = "PublishDate is required.")]
        public string publishDate { get; set; }

        [Required(ErrorMessage = "Topic is required.")]
        public string topic { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [AllowHtml]
        public string content { get; set; }
        public List<SelectListItem> TopicsList { get; set; }
        [Required(ErrorMessage = "Type is required.")]
        public bool type { get; set;}
    }


}