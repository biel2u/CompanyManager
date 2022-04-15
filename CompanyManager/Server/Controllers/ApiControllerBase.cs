using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase
    {
    }
}
