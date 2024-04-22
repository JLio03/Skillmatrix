using CMI.Skillmatrix.Components.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class AdminAuthorizationRequirement : IAuthorizationRequirement
{
    public bool IsAdminRequired { get; }

    public AdminAuthorizationRequirement(bool isAdminRequired)
    {
        IsAdminRequired = isAdminRequired;
    }
}

public class AdminAuthorizationHandler : AuthorizationHandler<AdminAuthorizationRequirement>
{
    private readonly SkillmatrixDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AdminAuthorizationHandler(SkillmatrixDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    // überprüft ob die Email des Angemeldeten Benutzers in der appsettings.json vorhanden ist
    // überprüft ob das Feld IsAdmin bei der Email des angemeldeten Benutzers = true ist
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAuthorizationRequirement requirement)
    {
        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        var userEmail = context.User.Identity.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            context.Fail();
            return;
        }

        var adminEmails = _configuration.GetSection("AdminUsers:Emails").Get<List<string>>();
        if (adminEmails != null && adminEmails.Contains(userEmail))
        {
            context.Succeed(requirement);
            return;
        }

        var isAdmin = await _dbContext.Mitarbeiter
            .Where(u => u.Email == userEmail && u.IsAdmin)
            .AnyAsync();

        if (isAdmin == requirement.IsAdminRequired)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}