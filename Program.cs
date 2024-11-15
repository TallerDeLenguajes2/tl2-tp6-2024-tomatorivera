using tl2_tp6_2024_tomatorivera.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Inyecto las dependencias
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();

// Habilito los servicios de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde HTTP, no JavaScript
    options.Cookie.IsEssential = true; // Necesario incluso si el usuario no acepta cookies
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// USAR SESIONES (DESPUES DEL BUILD)
app.UseSession();

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
