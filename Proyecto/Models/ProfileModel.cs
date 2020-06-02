using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Proyecto.Models
{
    public class ProfileModel
    {
        public int memberId { get; set; }

        [Required(ErrorMessage = "Please provide a name")]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please provide a lastname")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Display(Name = "Birthdate")]
        [DataType(DataType.Date)]
        public string birthDate { get; set; }

        [Display(Name = "Age")]
        public int age { get; set; }

        [Required(ErrorMessage = "Please provide a city")]
        [Display(Name = "City")]
        public string addressCity { get; set; }

        [Required(ErrorMessage = "Please provide a country")]
        [Display(Name = "Country")]
        public string addressCountry { get; set; }

        [Display(Name = "Hobbies")]
        public string hobbies { get; set; }

        [Display(Name = "Languages")]
        public string languages { get; set; }

        [Required(ErrorMessage = "Please provide an e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Mobile")]
        public string mobile { get; set; }

        [Display(Name = "Job")]
        public string job { get; set; }

        [Display(Name = "Member rank")]
        public string memberRank { get; set; }

        [Display(Name = "Points")]
        public int points { get; set; }

    }
}