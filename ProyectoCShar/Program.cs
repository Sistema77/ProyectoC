using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
////////////////////////////

// Inyectar el String de la conexion a la base de datos y crear la conexion
builder.Services.AddDbContext<ModelContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
);
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
