using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string AgentName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [StringLength(250)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter MobileNo")]
        [Display(Name = "Mobile no")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number Must be 10 numbers")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile Number Contains only Numbers")]
        public string Mobile { get; set; }
        [Required(ErrorMessage ="PAN Number is Required")]
        [Display(Name ="PAN Number")]
        //[RegularExpression (@"[A-Z]{5}[0-9]{4}[A-Z]{1}",ErrorMessage ="Enter Valid PAN Number")]
        public string PAN { get; set; }
        [Required ]
        [RegularExpression (@"^[0-9]*$", ErrorMessage ="Enter Valid Aadhar")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Enter Valid Aadhar")]

        public string Aaadhar { get; set; }
        public string AccountHolderName { get; set; }
        [Required]
        [RegularExpression (@"^\d{9,18}$",ErrorMessage ="Enter Valid Account Nmuber") ]
        public string BankAccountNumber { get; set; }
        [Required ]
        [RegularExpression (@"[A-Z|a-z]{4}[0][\d]{6}$",ErrorMessage ="Enter Valid IFSC Code")]
        public string IFSCCode { get; set; }
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Password Must be 8 letters")]
        [Required]
        public string Password { get; set; }
    }
}
