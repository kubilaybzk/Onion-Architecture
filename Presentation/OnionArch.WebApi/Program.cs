using FluentValidation.AspNetCore;
using OnionArch.Application.Validators.Product_Validators;
using OnionArch.infrastructure.Filters;
using OnionArch.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Burada fulent application için düzenleme yapıyoruz.





builder.Services.AddControllers(options => options.Filters.Add<ValidationFilters>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistanceServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
)) ;

//WithOrigins(
//"http://localhost:3000",
//    "https://localhost:3000",
//    "https://0.0.0.0:3000",
//    "http://0.0.0.0:3000",
//    "https://127.0.0.1:3000",
//    "http://127.0.0.1:3000",
//    "http://127.0.0.1:5500",
//    "https://127.0.0.1:5500"
//     )

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

