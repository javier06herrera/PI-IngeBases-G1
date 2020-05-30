using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class ReviewsModel
    {
        [Display(Name = "Member Identification")]
        [Required(ErrorMessage = "Please insert the identification of the member")]
        public int memberId { get; set; }

        [Display(Name = "Article Identification")]
        [Required(ErrorMessage = "Please insert the identification of the article")]
        public int articleId { get; set; }

        [Display(Name = "Status of revision")]
        public string status { get; set; } //checked -- not checked
    }
}