using System.ComponentModel.DataAnnotations;
using System.Configuration; //Para usar listas
using System.Data.SqlClient; // Para usar Listas
using System.Collections.Generic; //Para usar Listas
using System.Web.Mvc; // Para usar SelectListItem

namespace Proyecto.Models
{
    public class QuestionModel
    {
        public int questionId { get; set; }

        [Display(Name = "Question")]
        [Required(ErrorMessage = "Type your question!")]
        public string question { get; set; }

        //[Display(Name = "Question")]
        //[Required(ErrorMessage = "Person who asks is required.")]
        //public int askedBy { get; set; }

        [Display(Name = "FAQ")]
        public string faq { get; set; } //posted -- not posted

        [Display(Name = "Answer")]
        [Required(ErrorMessage = "Answer is required.")]
        [AllowHtml]
        public string answer { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status is required.")]
        public string status { get; set; }

        public List<SelectListItem> TopicsList { get; set; }        
    }
}