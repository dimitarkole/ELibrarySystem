namespace ELibrarySystem.Web.Areas.Administration.Controllers
{
    using ELibrarySystem.Common;
    using ELibrarySystem.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
