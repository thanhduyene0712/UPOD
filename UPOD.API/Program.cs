using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Extension;
using System.Text.Json.Serialization;
using UPOD.REPOSITORIES.Models;
using UPOD.SERVICES.Handlers;
using UPOP.SERVICES.App_Start;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigns";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});
// Add services to the container.


//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<Database_UPODContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureFilter<ErrorHandlingFilter>();
builder.Services.JsonFormatConfig();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureDI();
builder.Services.ConfigureServiceWorkers();
builder.Services.ConfigDataProtection();
var port = Environment.GetEnvironmentVariable("PORT");
builder.WebHost.UseUrls("http://*:" + port);
var app = builder.Build();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);


app.UseAuthorization();

app.MapControllers();

app.Run();