using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class WritesModel
    {
        [Display(Name = "Member's Email")]
        [Required(ErrorMessage = "Please insert the email of the member")]
        public string email { get; set; }

        [Display(Name = "Article Identification")]
        [Required(ErrorMessage = "Please insert the identification of the article")]
        public int articleId { get; set; }
    }
}