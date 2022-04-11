using CompanyManager.Server.Data;
using CompanyManager.Server.Helpers;
using CompanyManager.Server.Models;
using CompanyManager.Server.Repositories;
using CompanyManager.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IOfferRepository, OfferRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IOfferService, OfferService>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddTransient<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddTransient<IAppointmentOfferRepository, AppointmentOfferRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
