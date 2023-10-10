using EcommerceApplication.Context;
using Microsoft.EntityFrameworkCore;
using EcommerceApplication.Service.Interface;
using EcommerceApplication.Service.Infrastructure;
using EcommerceApplication.Models;
using EcommerceApplication.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using EcommerceApplication.Models.EmailModel;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// For Entity Framework
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
//: 'Unable to find the required services. Please add all the required services by calling 'IServiceCollection.
//AddRazorPages' inside the call to 'ConfigureServices(...)' in the application startup code.' to resolve these error 
// these is the very important to add app.MapRazorPages(); middleware
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ECommDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ECommmvc")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ECommDbContext>();
//In Identtity Framework send the mockup link on RegisterConfirmation Link and this link verify using the "options => options.SignIn.RequireConfirmedAccount = true" code
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ECommDbContext>()
               .AddDefaultTokenProviders();
//builder.Services.Configure<IdentityOptions>(
//              opts => opts.SignIn.RequireConfirmedEmail = true
//              );

builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
builder.Services.AddScoped<IGenericRepository<CategoryDto>, GenericRepository<CategoryDto>>();
builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped<IGenericRepository<ProductDto>, GenericRepository<ProductDto>>();
builder.Services.AddScoped<IEmailService, EmailSender>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Add Email Configs
var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                 options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                     };
                 });

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

app.UseAuthorization();


app.MapRazorPages();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
