
namespace ELibrarySystem.Web.ViewModels.HomeViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using ELibrarySystem.Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterViewModel
#pragma warning restore SA1649 // File name should match first type name
    {

        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = "Моля въведете правилен email адрес!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Дължината на паролата {0} трябва да бъде между {2} и {1} знака!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Нова парола")]
        [MaxLength(20)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтори новата паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат!")]
        public string ConfirmPassword { get; set; }


    }
}