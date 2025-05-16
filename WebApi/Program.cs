using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repository.EFCore;
using Services.Contracts;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true; //i�erik pazarl??na a�?k
    config.ReturnHttpNotAcceptable = true;  //istemci ile payla?t?k 406 kabul etmeyince verir.

})
    .AddCustomCsvFormatter()//csv format?nda �?kt? veren custom 
    .AddXmlDataContractSerializerFormatters() //art?k xml format?nda da istek d�nebilir.
    .AddApplicationPart(typeof(Presentation.AssemblyRefence).Assembly).AddNewtonsoftJson(); //PresentationLayer eklendi.

//builder.Services.AddScoped<ValidationFilterAttribute>();// IoC

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerManager();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);  //b�t�n hepsi handlerdan ge�ti?i i�in hatalar i�in try catche gerek kalm?yor.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHsts(); //Bu siteye sadece HTTPS �zerinden ba?lan. HTTP'ye izin vermez
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
