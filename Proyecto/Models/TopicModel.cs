using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class TopicModel
    {
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please insert a category")]
        public string subject_category { get; set; }

        [Display(Name = "Topic Name")]
        [Required(ErrorMessage = "Please insert a name for the topic")]
        public string subject_topicName { get; set; }
    }
}