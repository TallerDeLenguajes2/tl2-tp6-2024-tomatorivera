using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AccessLevelAttribute : Attribute, IAuthorizationFilter
{
    private readonly int _requiredLevel;

    public AccessLevelAttribute(AccessLevel requiredLevel)
    {
        _requiredLevel = (int) requiredLevel;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var accessLevel = context.HttpContext.Session.GetInt32("AccessLevel");
        if (accessLevel == null || accessLevel < _requiredLevel)
        {
            // Redirige al usuario a una página de acceso denegado o login
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
        }
    }
}