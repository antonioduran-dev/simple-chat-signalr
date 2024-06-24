using DB.Data;
using Microsoft.EntityFrameworkCore;
using PruebaChatSIgnalR.App.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// inject Context
builder.Services.AddDbContext<ChatDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection"))
);

// add signalR
builder.Services.AddSignalR();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// add map for ChatHub
app.MapHub<ChatHub>("/chatHub");

app.Run();
