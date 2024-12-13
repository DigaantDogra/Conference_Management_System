using ConferenceManagement.Data;
using ConferenceManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<PayPalService>();
builder.Services.AddHttpClient<OpenStreetMapService>();
builder.Services.AddScoped<PayPalService>();
builder.Services.AddScoped<OpenStreetMapService>();
builder.Services.AddControllersWithViews();

// Configure MySQL connection
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Add MVC default route

app.MapRazorPages();

app.Run();
