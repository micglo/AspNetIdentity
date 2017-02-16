using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.Model.BindingModel.Role
{
    public class RoleBindingModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}