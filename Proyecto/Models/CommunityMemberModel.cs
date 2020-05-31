using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class CommunityMemberModel
    {
        [Display(Name = "Member Identification")]
        public int memberId { get; set; }

        [Display(Name = "Member Name")]
        [Required(ErrorMessage = "Please insert the first name of the member")]
        public string name { get; set; }

        [Display(Name = "Member Last Name")]
        [Required(ErrorMessage = "Please insert the last name id of the member")]
        public string lastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm dd yyyy}")]
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Please provide your birth date")]
        public int birthDate { get; set; }

        [Display(Name = "Member's Age")]
        [Required(ErrorMessage = "Please insert the age of the member")]
        public int age { get; set; }

        public string publishDate { get; set; }
        [Display(Name = "City of the Member")]
        [Required(ErrorMessage = "Please insert the city where the member lives")]
        public string address_city { get; set; }

        [Display(Name = "Country of the Member")]
        [Required(ErrorMessage = "Please insert the country where the member lives")]
        public string address_country { get; set; }

        [Display(Name = "Member's Hobbies")]
        [Required(ErrorMessage = "Please insert the hobbies of the member separated by commas")]
        public string hobbies { get; set; }

        [Display(Name = "Member's Languages")]
        [Required(ErrorMessage = "Please insert the languages of the member separated by commas")]
        public string languages { get; set; }

        [Display(Name = "Member's Email")]
        [Required(ErrorMessage = "Please insert the email of the member")]
        public string email { get; set; }

        [Display(Name = "Member's Phonne Number")]
        [Required(ErrorMessage = "Please insert the phone number of the member")]
        public string phoneNumber { get; set; }

        [Display(Name = "Work Informartion")]
        [Required(ErrorMessage = "Please insert the job experience of the member")]
        public string workInformation { get; set; }

        [Display(Name = "Member's Type")]
        [Required(ErrorMessage = "Please insert the type of the member")]
        public string typeOfMember { get; set; }

        [Display(Name = "Member's Qualifiations")]
        public int totalQualification { get; set; }


    }
}