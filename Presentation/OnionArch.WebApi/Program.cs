using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using OnionArch.Application;
using OnionArch.Application.Validators.Product_Validators;
using OnionArch.infrastructure;
using OnionArch.infrastructure.Filters;
using OnionArch.infrastructure.Services.Storage.LocalStorage;
using OnionArch.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Burada fulent application için düzenleme yapıyoruz.


//Json sorununu çözmek için ;

//builder.Services.AddControllers().AddJsonOptions(x =>
//   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);



builder.Services.AddControllers(options => options.Filters.Add<ValidationFilters>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddStorage<LocalStorage>();

builder.Services.AddInfrastructureServices();
builder.Services.AddPersistanceServices();
builder.Services.AddApplicationServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(
"http://localhost:3000",
    "https://localhost:3000",
    "https://0.0.0.0:3000",
    "http://0.0.0.0:3000",
    "https://127.0.0.1:3000",
    "http://127.0.0.1:3000",
    "http://127.0.0.1:5500",
    "https://127.0.0.1:5500"
     ).AllowAnyHeader().AllowAnyMethod()
)) ;



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
    app.UseStaticFiles();
}
app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

