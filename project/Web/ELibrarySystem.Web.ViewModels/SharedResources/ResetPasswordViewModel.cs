namespace ELibrarySystem.Web.ViewModels.SharedResources
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class ResetPasswordViewModel
    {
        public ResetPasswordViewModel()
        {
            this.OldPassword = null;
            this.NewPassword = null;
            this.ConfirmNewPassword = null;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Стара парола")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Дължината на паролата {0} трябва да бъде между {2} и {1} знака!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        [MaxLength(20)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтори паролата")]
        [Compare("NewPassword", ErrorMessage = "Паролите не съвпадат!")]
        public string ConfirmNewPassword { get; set; }
    }
}
