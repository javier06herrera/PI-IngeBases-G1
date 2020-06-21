using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Proyecto.Models
{
    public class ReviewsModel
    {

        [Display(Name = "Article Identification")]
        [Required(ErrorMessage = "Please insert the identification of the article")]
        public int articleId { get; set; }

        [Display(Name = "Member email")]
        [Required(ErrorMessage = "Please insert the email of the member")]
        public string email { get; set; }

        [Display(Name = "Comments of the review member")]
        public string comments { get; set; } //checked -- not checked

        [Display(Name = "General Overview")]
        [Required(ErrorMessage = "Please insert the overview of the reviewer")]
        public int generalOpinion { get; set; }

        [Display(Name = "Community Contribution")]
        [Required(ErrorMessage = "Please insert the community contribution of this article")]
        public int communityContribution { get; set; }

        [Display(Name = "Article Structure")]
        [Required(ErrorMessage = "Please insert the article structure of this article")]
        public int articleStructure { get; set; }

        [Display(Name = "Total Grade")]
        [Required(ErrorMessage = "Please insert the total grade of this article")]
        public int totalGrade { get; set; }

        [Display(Name = "Status of revision")]
        public string state { get; set; } //checked -- not checked

        
    }
    
}

     
