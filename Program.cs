using CollaborativeBoardApi.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Agregar servicios de SignalR
builder.Services.AddSignalR()
    .AddMessagePackProtocol(); // Usar MessagePack para mejor rendimiento

// Agregar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200") // URL de tu app Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Agregar documentación de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("CorsPolicy");

app.UseAuthorization();

// Configurar rutas para controladores API
app.MapControllers();

// Configurar endpoint para el hub de SignalR
app.MapHub<BoardHub>("/boardHub");

app.Run();