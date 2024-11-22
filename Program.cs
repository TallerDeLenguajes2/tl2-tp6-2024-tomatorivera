using Models;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Registro los repositorios como dependencias
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepositoryImpl>();
builder.Services.AddScoped<IRepository<Producto>, ProductoRepositoryImpl>();
builder.Services.AddScoped<IRepository<Cliente>, ClienteRepositoryImpl>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.Run();
