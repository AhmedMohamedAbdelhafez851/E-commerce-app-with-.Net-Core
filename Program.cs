using LapShop.Bl;
using LapShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LapShopContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options=>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<LapShopContext>();

builder.Services.AddScoped<ICategories,ClsCategories>();
builder.Services.AddScoped<IItems, ClsItems>();
builder.Services.AddScoped<IItemTypes, ClsItemTypes>();
builder.Services.AddScoped<IOs, ClsOs>();
builder.Services.AddScoped<IItemImages, ClsItemImages>();
builder.Services.AddScoped<ISalesInvoice, ClsSalesInvoice>();
builder.Services.AddScoped<ISalesInvoiceItems, ClsSalesInvoiceItems>();
//builder.Services.AddScoped<VmHomePage>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    options.LoginPath = "/Users/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
    name: "LandingPages",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "ali",
    pattern: "ali/{controller=Home}/{action=Index}/{id?}");



}
);

app.Run();
