using EntertainmentApp.Application.Services;
using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.Infrastructure.Data;
using EntertainmentApp.Infrastructure.Identity;
using EntertainmentApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register application services and repositories
builder.Services.AddScoped<IEntertainmentRepository, EntertainmentRepository>();
builder.Services.AddScoped<ICreatorRepository, CreatorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserLikeRepository, UserLikeRepository>();

// Configure database connection
string conString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conString));

// Configure Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManageCatalogPolicy", policy =>
        policy.RequireClaim("CanManageCatalog", "true"));
});

var app = builder.Build();

// Seed initial data
await SeedData.EnsureSeedData(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

public static class SeedData
{
    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure roles exist
        if (!await roleManager.RoleExistsAsync("Administrator"))
        {
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
        }

        // Create user with CanManageCatalog claim
        var userWithClaim = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com", FirstName = "Admin", LastName = "User" };
        if (await userManager.FindByNameAsync(userWithClaim.UserName) == null)
        {
            await userManager.CreateAsync(userWithClaim, "Password123!");
            await userManager.AddToRoleAsync(userWithClaim, "Administrator");
            await userManager.AddClaimAsync(userWithClaim, new Claim("CanManageCatalog", "true"));
        }

        // Create user without CanManageCatalog claim
        var userWithoutClaim = new ApplicationUser { UserName = "user@user.com", Email = "user@user.com", FirstName = "Regular", LastName = "User" };
        if (await userManager.FindByNameAsync(userWithoutClaim.UserName) == null)
        {
            await userManager.CreateAsync(userWithoutClaim, "Password123!");
        }
    }
}
