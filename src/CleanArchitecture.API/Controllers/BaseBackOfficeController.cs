using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/backoffice/[controller]")]
    [ApiController]
    //[Authorize]
    public class BaseBackOfficeController : Controller
    {
    }
}
