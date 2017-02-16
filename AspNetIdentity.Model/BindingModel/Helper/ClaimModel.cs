using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.Model.BindingModel.Helper
{
    public class ClaimModel
    {
        [Required]
        [Display(Name = "Claim Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Claim Value")]
        public string Value { get; set; }
    }
}