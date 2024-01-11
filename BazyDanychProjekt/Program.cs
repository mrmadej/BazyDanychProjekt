using Microsoft.EntityFrameworkCore;
using BazyDanychProjekt.Data;
using Microsoft.AspNetCore.Identity;
using BazyDanychProjekt.Models;
using BazyDanychProjekt.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHoteleService, HoteleService>();
builder.Services.AddAuthentication();
builder.Services.AddIdentity<Uzytkownik, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDistributedMemoryCache(); // lub inny dostawca pamiêci podrêcznej
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // czas, po którym sesja wygaœnie
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Uzytkownik>>();

    var roleExists = roleManager.RoleExistsAsync("Admin").Result;

    if (!roleExists)
    {
        var roleResult = roleManager.CreateAsync(new IdentityRole("Admin")).Result;

        if (!roleResult.Succeeded)
        {
            // Obs³uga b³êdów
        }
    }
    var roleExistsUser = roleManager.RoleExistsAsync("Uzytkownik").Result;

    if (!roleExistsUser)
    {
        var roleResult = roleManager.CreateAsync(new IdentityRole("Uzytkownik")).Result;

        if (!roleResult.Succeeded)
        {
            // Obs³uga b³êdów
        }
    }
    var adminUser = userManager.FindByNameAsync("admin").Result;

    if (adminUser == null)
    {
        
        adminUser = new Uzytkownik { UserName = "admin", Rola = "Admin", Haslo = "Password1!", Login = "admin" , Imie = "", Nazwisko = "" };

        var result = userManager.CreateAsync(adminUser, "Password1!").Result;

        if (!result.Succeeded)
        {
            // Obs³uga b³êdów
        }
    }

    // Przypisanie roli "Admin" do u¿ytkownika "admin"
    var addToRoleResult = userManager.AddToRoleAsync(adminUser, "Admin").Result;

    if (!addToRoleResult.Succeeded)
    {
        // Obs³uga b³êdów
    }
}



var app = builder.Build();

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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Konto}/{action=Logowanie}/{id?}");

app.Run();
