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
    config.RespectBrowserAcceptHeader = true; //içerik pazarl??na aç?k
    config.ReturnHttpNotAcceptable = true;  //istemci ile payla?t?k 406 kabul etmeyince verir.

})
    .AddCustomCsvFormatter()//csv format?nda ç?kt? veren custom 
    .AddXmlDataContractSerializerFormatters() //art?k xml format?nda da istek dönebilir.
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
app.ConfigureExceptionHandler(logger);  //bütün hepsi handlerdan geçti?i için hatalar için try catche gerek kalm?yor.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHsts(); //Bu siteye sadece HTTPS üzerinden ba?lan. HTTP'ye izin vermez
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
