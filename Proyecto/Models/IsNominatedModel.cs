using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc; // Para usar SelectListItem

namespace Proyecto.Models
{
    public class IsNominatedModel
    {
        [Display(Name = "Answer to Request")]
        public string answer { get; set; }

        [Display(Name = "Comments")]
        public string comments { get; set; }


        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "articleID")]
        public int articleId { get; set; }

        public List<SelectListItem> options = new List<SelectListItem>();

        public bool selected { get; set; }

        public List<SelectListItem> optionList
        {
            get
            {
                if (options.Count < 1)
                {
                    options.Add(new SelectListItem() { Text = "I want to review this article", Value = "Accept" });
                    options.Add(new SelectListItem() { Text = "I have a conflict with this article", Value = "Conflict" });
                    options.Add(new SelectListItem() { Text = "I dont want to review this article", Value = "Reject" });

                }

                return options;
            }
        }

    }

    public class IsNominatedModelDetail
    {
        public List<IsNominatedModel> NominatedDetails { get; set; }
    }
}