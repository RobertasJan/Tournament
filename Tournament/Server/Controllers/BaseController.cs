using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Tournament.Server.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// AutoMapper
        /// </summary>
        protected IMapper Mapper =>
            HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}
