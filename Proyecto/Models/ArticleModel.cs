using System.ComponentModel.DataAnnotations;
using System.Configuration; //Para usar listas
using System.Data.SqlClient; // Para usar Listas
using System.Collections.Generic; //Para usar Listas
using System.Web.Mvc; // Para usar SelectListItem

namespace Proyecto.Models
{
    public class ArticleModel
    {

        [Display(Name = "Article ID")]
        public int articleId { get; set; }

        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please provide a name")]
        public string name { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please provide a type")]
        public string type { get; set; }
        //[Required(ErrorMessage = "Topic is required.")]
        //public string Topic { get; set; }

        [Display(Name = "Abstract")]
        [Required(ErrorMessage = "Please provide an abstract")]
        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm dd yyyy}")]
        [Display(Name = "Publish Date")]
        [Required(ErrorMessage = "Please provide a date")]
        public string publishDate { get; set; }

        //Should be comented///////////////////////////////////////////////////////////////
        [Display(Name = "Topic")]
        //[Required(ErrorMessage = "Please provide a topic")]
        public string topicName { get; set; }
        //Should be comented///////////////////////////////////////////////////////////////

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Please provide a content")]
        [AllowHtml]
        public string content { get; set; }
        public List<SelectListItem> TopicsList { get; set; }

        [Display(Name = "Base Grade")]
        public int baseGrade { get; set; }

        [Display(Name = "Access Count")]
        public int accessCount { get; set; }

        [Display(Name = "Likes Count")]
        public int likesCount { get; set; }

        [Display(Name = "Dislikes Count")]
        public int dislikesCount { get; set; }

        [Display(Name = "Like Balance")]
        public int likeBalance { get; set; }


    }


}