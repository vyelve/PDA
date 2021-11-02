using PDA.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.ViewModel
{
    public class UserViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email ID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public GenderEnum GenderEnum { get; set; }

        [Required(ErrorMessage = "Phone No Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Phone No")]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "Login Name Required")]
        public string LoginName { get; set; }

        [MinLength(8, ErrorMessage = "Minimum Password must be 8 in characters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserPhotoPath { get; set; }


        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public IEnumerable<UserViewModel> TblUsers { get; set; }
        public string Message { get; set; }


    }
}
