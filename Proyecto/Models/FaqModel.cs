using System.ComponentModel.DataAnnotations;
using System.Configuration; //Para usar listas
using System.Data.SqlClient; // Para usar Listas
using System.Collections.Generic; //Para usar Listas
using System.Web.Mvc; // Para usar SelectListItem

namespace Proyecto.Models
{
    public class FaqModel
    {
        public int questionId { get; set; }

        [Display(Name = "Question")]

        [Required(ErrorMessage = "Question is required.")]
        public string question { get; set; }

        //[Display(Name = "Category")]
        //[Required(ErrorMessage = "Category is required.")]
        //public string category { get; set; }

        [Display(Name = "Answer")]
        [Required(ErrorMessage = "Answer is required.")]
        [AllowHtml]
        public string answer { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public bool status { get; set; }

        public List<SelectListItem> TopicsList { get; set; }        
    }
}