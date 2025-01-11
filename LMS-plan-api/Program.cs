using DotNetEnv;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.PlanDbContext;
using DAL.Repositories;
using Logic;
using Logic.IRepositories;
using Logic.IServices;
using Logic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Env.Load();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(corsPolicyBuilder =>
	{
		corsPolicyBuilder.WithOrigins()
			.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials()
			.SetIsOriginAllowed(_ => true);
	});
});

var connectionString =
	$"Server={Env.GetString("DB_HOST")};Port={Env.GetString("DB_PORT")};Database={Env.GetString("DB_NAME")};User={Env.GetString("DB_USER")};Password={Env.GetString("DB_PASSWORD")};";

builder.Services.AddDbContext<PlanDbContext>(options =>
	options.UseMySql(
		builder.Configuration.GetConnectionString(connectionString),
		new MySqlServerVersion(new Version(Env.GetInt("SQL_MAJOR"), Env.GetInt("SQL_MINOR"), Env.GetInt("SQL_BUILD")))
	)
);

//!TODO auth


builder.Services.AddScoped<IPlanActivityRepository, PlanActivityRepository>();
builder.Services.AddScoped<IPlanDateRepository, PlanDateRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IPlanService, PlanService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
