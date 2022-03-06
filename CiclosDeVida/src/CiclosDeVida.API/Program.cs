using CiclosDeVida.API.Model;
using CiclosDeVida.API.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICicloDeVidaSingleton, CicloDeVida>();
builder.Services.AddScoped<ICicloDeVidaScoped, CicloDeVida>();
builder.Services.AddTransient<ICicloDeVidaTransient, CicloDeVida>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


ICicloDeVidaSingleton singleton = app.Services.GetService<ICicloDeVidaSingleton>();
//ICicloDeVidaScoped scoped = app.Services.GetRequiredService<ICicloDeVidaScoped>();
ICicloDeVidaTransient transient = app.Services.GetRequiredService<ICicloDeVidaTransient>();


app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/singleton", (ICicloDeVidaSingleton singleton) =>
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
    

app.MapGet("/transient", (ICicloDeVidaTransient transient) =>
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
