using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApi.Controllers
{
    [Tags("user.partner")]
    public class UserPartnerController : ControllerBase
    {
        private readonly ILogger<UserPartnerController> _logger;
        private readonly IDistributedCache cache;

        public UserPartnerController(ILogger<UserPartnerController> logger, IDistributedCache distributedCache)
        {
            cache = distributedCache;
            _logger = logger;
        }

        [HttpPost("/api.user.partner.rememberMe")]
        public async Task<ActionResult> RememberMe([BindRequired][FromQuery]string code)
        {;
            await cache.SetStringAsync("code", code, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });

            return Ok();
        }
    }
}
