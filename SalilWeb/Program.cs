using Microsoft.EntityFrameworkCore;
using Salil.DataAccess;
using Salil.DataAccess.Repository;
using Salil.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Salil.Utility;
using Stripe;
using Salil.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//add db context as service which we can access dependency injection and based on connection string it will access the db
builder.Services.AddDbContext<ApplicationDBContext>(options=> options.UseSqlServer(
    builder.Configuration.GetConnectionString("BlazorConnectionString")
    ));

//For Stripe Payment Gateway
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

//for custom identity
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

//for email singleton implementation
builder.Services.AddSingleton<IEmailSender, EmailSender>();

//for Seed Database

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

//for unit of works repository
builder.Services.AddScoped<IUnitOfWorks, UnitOfWork>();

//For login/logout/AccessDenied Path since we have added custom identity
builder.Services.ConfigureApplicationCookie(options=>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

//for session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{ 
    options.IdleTimeout= TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly= true;
    options.Cookie.IsEssential= true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//call seed databse
SeedDatabase();

string key = builder.Configuration.GetSection("Stripe:SecureKey").Get<string>();
StripeConfiguration.ApiKey = key;
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.MapControllers();

app.Run();

//Seed Database
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}