using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Comments")]
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

        public List<SelectListItem> options = new List<SelectListItem>();

        public List<SelectListItem> optionList
        {
            get
            {
                if (options.Count < 1 )
                {
                    options.Add(new SelectListItem() { Text = "1", Value = "1" });
                    options.Add(new SelectListItem() { Text = "2", Value = "2" });
                    options.Add(new SelectListItem() { Text = "3", Value = "3" });
                    options.Add(new SelectListItem() { Text = "4", Value = "4" });
                    options.Add(new SelectListItem() { Text = "5", Value = "5" });

                }

                return options;
            }
        }
        

    }
    
    
}

     
