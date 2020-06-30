using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Proyecto.Models
{
    public class ProfileModel
    {
        //public int memberId { get; set; }

        [Required(ErrorMessage = "Please provide your e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please provide your name")]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please provide your lastname")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Display(Name = "Birthdate")]
        [DataType(DataType.Date)]
        public string birthDate { get; set; }

        [Display(Name = "Age")]
        public int age { get; set; }

        [Required(ErrorMessage = "Please provide the city where you live")]
        [Display(Name = "City")]
        public string addressCity { get; set; }

        [Required(ErrorMessage = "Please provide the country where you live")]
        [Display(Name = "Country")]
        public string addressCountry { get; set; }

        [Display(Name = "Hobbies")]
        public string hobbies { get; set; }

        [Display(Name = "Languages")]
        public string languages { get; set; }

        [Required(ErrorMessage = "Please provide your mobile number")]
        [Display(Name = "Mobile")]
        public string mobile { get; set; }

        [Required(ErrorMessage = "Please enter your job title")]
        [Display(Name = "Job")]
        public string job { get; set; }

        [Display(Name = "Member rank")]
        public string memberRank { get; set; }

        [Display(Name = "Points")]
        public int points { get; set; }

        [Required(ErrorMessage = "Please describe your skills")]
        [Display(Name = "Skills")]
        public string skills { get; set; }

        public int articleCount { get; set; }

        public List<SelectListItem> ProfileList { get; set; }

        [Required(ErrorMessage = "Please provide your password.")]
        [Display(Name = "Password")]
        public string password { get; set; }



    }
}
