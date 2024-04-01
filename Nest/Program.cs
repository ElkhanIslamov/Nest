using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nest.Contexts;
using Nest.Models;

var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddControllersWithViews();
 builder.Services.AddDbContext<NestDbContext>(options =>
   {
       options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
   });

builder.Services.AddIdentity<AppUser,IdentityRole>(options=>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;

})
.AddEntityFrameworkStores<NestDbContext>();

void AddEntityFrameworkStores<T>()
{
    throw new NotImplementedException();
}

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}"
   );

app.UseAuthentication();
app.UseAuthorization();


app.Run();
