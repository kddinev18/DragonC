
using DragonC.API.Controllers;
using DragonC.API.Data;
using DragonC.API.Models.Entities;
using DragonC.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DragonC.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

			// DB Context
			object value = builder.Services.AddDbContext<DragonCDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Identity (no roles)
			builder.Services.AddIdentity<User, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;
			})
			.AddEntityFrameworkStores<DragonCDbContext>()
			.AddDefaultTokenProviders();

			// JWT Auth
			var jwtKey = builder.Configuration["Jwt:Key"];
			var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				// Optional: Add JWT Auth support in Swagger
				options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Description = "Enter 'Bearer' followed by a space and your JWT token."
				});

				options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
						new string[] {}
					}
	});
			});
			
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true
				};
			});

			// Services
			builder.Services.AddScoped<JwtService>();
			builder.Services.AddScoped<IProjectService, ProjectService>();
			builder.Services.AddScoped<ICodeConfigurationService, CodeConfigurationService>();
			builder.Services.AddControllers();

			var app = builder.Build();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI();

			app.MapControllers();
			app.Run();

			//var builder = WebApplication.CreateBuilder(args);

			//// Add services to the container.

			//builder.Services.AddControllers();
			//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			//builder.Services.AddOpenApi();

			//var app = builder.Build();

			//// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			//    app.MapOpenApi();
			//}

			//app.UseHttpsRedirection();

			//app.UseAuthorization();


			//app.MapControllers();

			//app.Run();
		}
    }
}
