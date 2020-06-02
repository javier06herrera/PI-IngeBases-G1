using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class SkillModel
    {
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please insert a category")]
        public string subjectCategory { get; set; }

        [Display(Name = "Skill Name")]
        [Required(ErrorMessage = "Please insert a name for the skill")]
        public string subjectSkillName { get; set; }
    }
}