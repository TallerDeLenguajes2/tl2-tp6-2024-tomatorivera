using Models;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Registro los repositorios como dependencias
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepositoryImpl>();
builder.Services.AddScoped<IRepository<Producto>, ProductoRepositoryImpl>();
builder.Services.AddScoped<IRepository<Cliente>, ClienteRepositoryImpl>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryImpl>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Habilito los servicios de sesiones
builder.Services.AddDistributedMemoryCache(); // Necesario para usar sesiones en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde HTTP, no JavaScript
    options.Cookie.IsEssential = true; // Necesario incluso si el usuario no acepta cookies
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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
