using Azure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace RWA_MVC_project.Filters
{
    public class LoginFilter : IAuthorizationFilter
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public LoginFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext!;

            if (!httpContext.Request.Cookies.ContainsKey("username"))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
