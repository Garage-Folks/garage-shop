using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using FineWoodworkingBasic.Util;
using FineWoodworkingBasic.Service;
using MudBlazor.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using FineWoodworkingBasic.Authentication.Provider;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using FineWoodworkingBasic.Authentication;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
Utilities.EstablishConnection("C:\\Users\\jaybo\\source\\repos\\garage-shop\\FineWoodworkingBasic\\DBConfig.ini");
=======
Utilities.EstablishConnection("C:\\Users\\nokil\\source\\repos\\garage-shop\\FineWoodworkingBasic\\dbConfig.ini");
>>>>>>> 45aac51 (loginFunctionality: Initial custom identity provider and custom authentication state provider)

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Register the login service
builder.Services.AddSingleton<LoginService>();
builder.Services.AddSingleton<AddBrandService>();
// Add the AppState class
builder.Services.AddScoped<AllStateInfoService>();
builder.Services.AddMudServices();

builder.Services.AddScoped<ProtectedSessionStorage>();

// Add identity types
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;

})
    .AddDefaultTokenProviders(); 

// Identity Services
builder.Services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();
string connectionString = Utilities.GetConnectionString();
builder.Services.AddTransient<SqlConnection>(e => new SqlConnection(connectionString));
builder.Services.AddTransient<UsersTable>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
