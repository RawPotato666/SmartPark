using University.Data;
using University.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// nastavi spremenljivko connectionString za .useSqlServer(connectionString)
var connectionString = builder.Configuration.GetConnectionString("SchoolContext");

// Add services to the container.
builder.Services.AddControllersWithViews();

// nadomesti stari .AddDbContext
builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(connectionString));

// prilagodi RequireConfirmedAccount = false in .AddRoles<IdentityRole>()
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SchoolContext>();
var app = builder.Build();

// Seed database using DbInitializer 
using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    DbInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.UseAuthorization();
// dodaj app.MapRazorPages(); (npr. za app.useAuthentication())
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
