using System.ComponentModel.DataAnnotations;

namespace Nest.ViewModels
{
    public class SubmitPasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
