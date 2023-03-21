using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Test2Migrate.Connection;
using Test2Migrate.Migrations.Locators;
using Test2Migrate.Migrations.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));


// Add services to the container.
builder.Services.AddScoped<IMongoMigrationsRunner, MongoMigrationsRunner>();
builder.Services.AddScoped<IMigrationHistoryService, MigrationHistoryService>();
builder.Services.AddScoped<IDatabaseTypeMigrationDependencyLocator, TypeMigrationLocator>();
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
