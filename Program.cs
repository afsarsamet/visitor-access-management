using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Hubs;
using WebApplication1.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("KardemirGuvenlik")
    .AddCookie("KardemirGuvenlik", options =>
    {
        
        options.LoginPath = "/Giris/Index";

       
        options.Cookie.Name = "KardemirAuth";
    });
builder.Services.AddDbContext<dbContextClass>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();
builder.Services.AddHostedService<WebApplication1.Services.SureKontrolIscisi>();


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
app.UseAuthentication(); 


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<AracHub>("/aracHub");


using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<dbContextClass>();

    var ilkAdminSifresi =
        builder.Configuration["InitialAdminPassword"];

    if (!context.Personel.Any() &&
        !string.IsNullOrWhiteSpace(ilkAdminSifresi))
    {
        var ilkAdmin = new Personel
        {
            AdSoyad = "Sistem Yöneticisi",
            SicilNo = "1000",
            TCNo = "11111111111",
            TelefonNo = "5555555555",
            AktifMi = true,
            rol = "Admin",
            sifre = BCrypt.Net.BCrypt.HashPassword(
                ilkAdminSifresi)
        };

        context.Personel.Add(ilkAdmin);
        context.SaveChanges();
    }
}

app.Run();