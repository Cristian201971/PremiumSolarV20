//using PanelSolar.Repositories;
//using Microsoft.Extensions.FileProviders;
using AspNetCore.ReCaptcha;
using Microsoft.Extensions.FileProviders;
using PanelSolar.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Register ReCaptcha
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptchaV3"));


// Add services to the container.
builder.Services.AddScoped<MailService>();
//builder.Services.AddSingleton<HelperPathProvider>();
//builder.Services.AddTransient<RepositoryJSON>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
//Nota: Antes de publicar se debe cambiar a  => IsProduction()
if (!app.Environment.IsDevelopment())
//if (!app.Environment.IsProduction())
{
    //app.UseExceptionHandler("../Error");
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Template")),
    RequestPath = new PathString("/MyTemplate")
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
