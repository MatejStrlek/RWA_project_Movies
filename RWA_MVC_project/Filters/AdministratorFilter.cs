using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RWA_MVC_project.Filters
{
    public class AdministratorFilter : IAuthorizationFilter
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public AdministratorFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext!;

            if (!httpContext.Request.Cookies.ContainsKey("username") || httpContext.Request.Cookies["username"] != "admin")
            {
                context.Result = new RedirectToActionResult("Forbidden", "Unauthorized", null);
            }
        }
    }
}
