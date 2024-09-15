using Core.Dao;
using Core.Negocios;
using Core.Servicios;

namespace ApiAntinarcoticos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Agregar la Interfaz y los Negocios
            builder.Services.AddScoped<IDataAccess, DataAccess>();
            builder.Services.AddScoped<ILoginManager, LoginManager>();
            builder.Services.AddScoped<IValorFlexibleManager, ValorFlexibleManager>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseCors(options =>
            {
                options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();

            app.Run();
        }
    }
}
