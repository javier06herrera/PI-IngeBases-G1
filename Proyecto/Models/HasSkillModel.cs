using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class HasSkillModel
    {
        [Display(Name = "Member Identification")]
        [Required(ErrorMessage = "Please insert the id of the member")]
        public int memberId { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please insert a category")]
        public string category { get; set; }

        [Display(Name = "Skill Name")]
        [Required(ErrorMessage = "Please insert a name for the skill")]
        public string skillName { get; set; }
    }
}