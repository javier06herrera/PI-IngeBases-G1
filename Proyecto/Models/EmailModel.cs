using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Configuration;


namespace Proyecto.Models
{
    public class EmailModel
    {
        [Display(Name = "Mail address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please provide mail address")]
        public string mail { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please enter a subject")]
        public string subject { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "Please input mail content")]
        [AllowHtml]
        public string message { get; set; }

    }
}