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

        [Display(Name = "Article")]

        [Required(ErrorMessage = "Please provide a name")]
        public string name { get; set; }

        //[Required(ErrorMessage = "Topic is required.")]
        //public string Topic { get; set; }

        [Required(ErrorMessage = "Please provide an abstract")]
        public string Abstract { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm dd yyyy}")]
        [Display(Name = "Publish Date")]
        [Required(ErrorMessage = "Please provide a date")]
        public string publishDate { get; set; }

        [Display(Name = "Topic")]
        [Required(ErrorMessage = "Please provide a topic")]
        public string topic { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Please provide a content")]
        [AllowHtml]
        public string content { get; set; }
        public List<SelectListItem> TopicsList { get; set; }
        [Required(ErrorMessage = "Please provide a type")]
        public bool type { get; set;}
    }


}