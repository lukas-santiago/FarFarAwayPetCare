using System.Text.Json.Serialization;
using Application.Configuration;
using Application.Middlewares;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
{
    builder.Services.AddDbContext<ApiContext>(options => options.UseInMemoryDatabase("DesafioDistribuicaoDosLucrosDB"));

    builder.Services.AddScoped<IDeviceService, DeviceService>();
    builder.Services.AddScoped<IDeviceConfigService, DeviceConfigService>();
    builder.Services.AddScoped<IDeviceDataService, DeviceDataService>();
}

var app = builder.Build();
{
    app.UseMiddleware(typeof(GlobalErrorHandlerMiddleware));
    app.UseCors(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // using (var scope = app.Services.CreateScope())
    // {
    //     scope.ServiceProvider.GetRequiredService<InitialDataGenerator>();
    // }

    // app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}