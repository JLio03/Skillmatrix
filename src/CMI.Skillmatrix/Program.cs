using CMI.Skillmatrix.Components;
using Microsoft.Identity.Web;
using CMI.Skillmatrix.Components.Data;
using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var defaultConnectionString = builder.Configuration.GetConnectionString("Default")
                              ?? throw new NullReferenceException("No connection string in config File...");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(connectionString: defaultConnectionString,
                         sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                         {
                             TableName = "Log", AutoCreateSqlTable = true
                         })
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureEntraID", options);
        options.Prompt = "select_account";
    });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<SkillmatrixDbContext>(options => options
    .UseLazyLoadingProxies() 
    .UseSqlServer(defaultConnectionString), ServiceLifetime.Transient);

builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Account/AccessDenied");

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<SkillkategorieService>();
builder.Services.AddTransient<SkillService>();
builder.Services.AddTransient<MitarbeiterService>();
builder.Services.AddTransient<MitarbeiterSkillService>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();
builder.Services.AddSingleton<AdminService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.Requirements.Add(new AdminAuthorizationRequirement(true));
    });
});

builder.Services.AddMudServices(options =>
{
    options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    options.SnackbarConfiguration.PreventDuplicates = false;
    options.SnackbarConfiguration.NewestOnTop = false;
    options.SnackbarConfiguration.ShowCloseIcon = true;
    options.SnackbarConfiguration.VisibleStateDuration = 5000;
    options.SnackbarConfiguration.HideTransitionDuration = 500;
    options.SnackbarConfiguration.ShowTransitionDuration = 500;
    options.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// stellt sicher, dass die Datenbank existiert und alle Migrationen durchgeführt sind
using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SkillmatrixDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
