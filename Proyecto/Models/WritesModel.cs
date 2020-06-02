using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class WritesModel
    {
        [Display(Name = "Member Identification")]
        [Required(ErrorMessage = "Please insert the identification of the member")]
        public int memberId { get; set; }

        [Display(Name = "Article Identification")]
        [Required(ErrorMessage = "Please insert the identification of the article")]
        public int articleId { get; set; }
    }
}