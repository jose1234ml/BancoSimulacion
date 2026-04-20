using BancoSimulacion.Components;
using BancoSimulacion.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS (Siempre antes del builder.Build)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrar el motor de simulación
builder.Services.AddSingleton<BankSimulationService>();

var app = builder.Build();

// 2. CONFIGURACIÓN DEL PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Si te sigue dando problemas el HTTPS en tu PC, puedes comentar esta línea temporalmente:
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();