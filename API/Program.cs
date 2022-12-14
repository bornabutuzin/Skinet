using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using API;
using Infrastructure;
using API.MiddleWare;
using Microsoft.AspNetCore.Cors;

internal class Program
{
    [EnableCors("CorsPolicy")]
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddAPIServices();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", builder =>
        {
            builder.WithOrigins("https://localhost:4200","https://localhost:3000").AllowAnyMethod().AllowAnyHeader();
        });
    });

        var app = builder.Build();
        app.UseMiddleware<ExceptionMiddleware>();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Initialise and seed database
            using (var scope = app.Services.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
                await initialiser.InitialiseAsync();
                await StoreContextSeed.SeedAsync(context);
            }
        }
        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}