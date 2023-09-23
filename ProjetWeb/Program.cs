using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using ProjetWeb.Models;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("_DbContext") ?? throw new InvalidOperationException("Connection string '_DbContext' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<_DbContext>().AddDefaultUI();



if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<_DbContext>(options =>
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=HelmoBilite;Trusted_Connection=True;"));
} else
{
    builder.Services.AddDbContext<_DbContext>(options =>
                options.UseSqlServer(@"url database"));
}

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<_DbContext>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(
    new StaticFileOptions { OnPrepareResponse = ctx => { const int durationInSeconds = 5;
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public, max-age=" + durationInSeconds;
    } }
    );

app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    DataInitializer.SeedRole(roleManager);
    DataInitializer.Seed(userManager);

}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
