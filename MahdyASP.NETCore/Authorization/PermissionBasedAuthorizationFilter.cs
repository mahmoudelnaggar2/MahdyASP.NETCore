using MahdyASP.NETCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MahdyASP.NETCore.Authorization;

public class PermissionBasedAuthorizationFilter(ApplicationDBContext dbContext) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var attribute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata
            .FirstOrDefault(x=> x is CheckPermissionAttribute);

        if (attribute != null)
        {
            if (context.HttpContext.User.Identity is not ClaimsIdentity claimIdentity || 
                !claimIdentity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }
            else
            {
                var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

                var haspermission = dbContext.Set<UserPermission>().Any(x => x.UserId == userId
                && x.PermissionId == attribute.Permission);

                if (!haspermission)
                {
                    context.Result = new ForbidResult();
                }
            }            
        }
    }
}
