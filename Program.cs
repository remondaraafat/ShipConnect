
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.Repository;
using ShipConnect.RepositoryContract;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            #region DbContext & Identity
            builder.Services.AddDbContext<ShipConnectContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ShipConnect"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ShipConnectContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // أو الدومين الفعلي لو deploy
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            #endregion

            #region Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowReactApp");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
