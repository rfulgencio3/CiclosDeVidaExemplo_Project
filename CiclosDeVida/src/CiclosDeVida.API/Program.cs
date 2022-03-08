using CiclosDeVida.API.Model;
using CiclosDeVida.API.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

//Configuração de serviços
builder.Services.AddSingleton<ICicloDeVidaSingleton, CicloDeVida>();
builder.Services.AddScoped<ICicloDeVidaScoped, CicloDeVida>();
builder.Services.AddTransient<ICicloDeVidaTransient, CicloDeVida>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Injeção de dependências das interfaces
ICicloDeVidaSingleton singleton = app.Services.GetService<ICicloDeVidaSingleton>();
ICicloDeVidaTransient transient = app.Services.GetRequiredService<ICicloDeVidaTransient>();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/singleton", () =>
    singleton.CicloDeVidaId);

app.MapGet("/scoped", () =>
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var services = serviceScope.ServiceProvider;
        var scoped = services.GetRequiredService<ICicloDeVidaScoped>();

        return scoped.CicloDeVidaId;
    }
});
    

app.MapGet("/transient", () =>
    transient.CicloDeVidaId);

app.MapGet("/ciclosdevida", (ICicloDeVidaSingleton singleton, ICicloDeVidaScoped scoped, ICicloDeVidaTransient transient) =>
{
    List<TipoCicloDeVida> tipoCicloDeVida = new List<TipoCicloDeVida>();
    
    tipoCicloDeVida.Add(new TipoCicloDeVida("Singleton", singleton.CicloDeVidaId));
    tipoCicloDeVida.Add(new TipoCicloDeVida("Scoped", scoped.CicloDeVidaId));
    tipoCicloDeVida.Add(new TipoCicloDeVida("Transient", transient.CicloDeVidaId));

    return JsonConvert.SerializeObject(tipoCicloDeVida, Formatting.Indented);
});

app.Run();
public class TipoCicloDeVida
{
    public TipoCicloDeVida(string tipo, Guid codigo)
    {
        Tipo = tipo;
        Codigo = codigo;
    }

    public string Tipo { get; set; }
    public Guid Codigo { get; set; }
}
