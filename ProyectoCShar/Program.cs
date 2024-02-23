using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Servicio;
using ProyectoCShar.Util;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
////////////////////////////

// Inyectar el String de la conexion a la base de datos y crear la conexion
builder.Services.AddDbContext<ModelContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//Usuarios
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicioImpl>();
builder.Services.AddScoped<IPasarADAO, PasarADAOImpl>();
builder.Services.AddScoped<IPasarADTO, PasarADTOImpl>();
//Util
builder.Services.AddScoped<IServicioEncriptar, ServicioEncriptarImpl>();
builder.Services.AddScoped<IServicioEmail, ServicioEmailImpl>();
//Cuentas
builder.Services.AddScoped<ICuentaServicio, CuentaServicioImplcs>();
builder.Services.AddScoped<ICuentaPasarADAO, CuentaPasarADAOImple>();
builder.Services.AddScoped<ICuentaPasarADTO, CuentaPasarADTOImple>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/auth/AccessDenied";
});
///////////////////////////

var app = builder.Build();

//////////////////////

// Hacer la Migracion
using (var scope = app.Services.CreateScope()) 
{
    var context = scope.ServiceProvider.GetRequiredService<ModelContext>();
    context.Database.Migrate();
}

/////////////////////

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
