using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Presentation.Controllers
{
    public class AuthController : ControllerBase
    {
        [Route("api/authentication")]
        [ApiController]
        public class AuthenticationController : ControllerBase
        {
            private readonly IServiceManager _service;
            public AuthenticationController(IServiceManager service) => _service = service;
        }
    }
}
