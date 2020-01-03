namespace ELibrarySystem.Web.ViewModels.HomeViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Моля въведете код!")]
        [Display(Name = "Код за потвържение")]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Дължината на паролата {0} трябва да бъде между {2} и {1} знака!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтори паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат!")]
        public string ConfirmPassword { get; set; }
    }
}
