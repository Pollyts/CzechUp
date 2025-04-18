using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CzechUp.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseController: ControllerBase
    {
        public BaseController() 
        { 
        
        }

        protected Guid UserGuid()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}
