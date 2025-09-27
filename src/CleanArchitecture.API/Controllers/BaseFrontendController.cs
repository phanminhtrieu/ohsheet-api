using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/frontend/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseFrontendController : Controller
    {
    }
}
