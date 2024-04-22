using Microsoft.AspNetCore.Authorization;

namespace CMI.Skillmatrix.Services
{
    public class AdminService(IAuthorizationService authorizationService, IHttpContextAccessor contextAccessor)
    {
        // Verwendet den AdminAuthorizationHandler, der das Requirement "AdminAuthorizationRequiremen" handeln kann
        public async Task<bool> IsAdminAsync()
        {
            var adminRequirement = new AdminAuthorizationRequirement(isAdminRequired: true);
            var authorizationResult = await authorizationService.AuthorizeAsync(contextAccessor.HttpContext.User, null, adminRequirement);

            return authorizationResult.Succeeded;
        }
    }
}
