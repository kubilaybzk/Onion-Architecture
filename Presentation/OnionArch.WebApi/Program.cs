using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnionArch.Application;
using OnionArch.Application.Validators.Product_Validators;
using OnionArch.infrastructure;
using OnionArch.infrastructure.Filters;
using OnionArch.infrastructure.Services.Storage.LocalStorage;
using OnionArch.Persistance;
using OnionArch.WebApi.Middlewares;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using SignalR;

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





/* SeriLog ile ilgili konfigürasyonlar */


//Tabloya yeni bir alan ekleyelim.

ColumnOptions columnOptions=new ColumnOptions();
columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    //Yeni bir alan oluşturup bu alanda kendimize özel allanlar ekliyoruz.
    new SqlColumn
    {
        ColumnName="EmailOrUserNameLogs",
        DataType=SqlDbType.NVarChar,
        DataLength=100
    }
};



Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/Logs.txt")
    .WriteTo.MSSqlServer(
    builder.Configuration.GetConnectionString("SqlConnectionString"),
    tableName: "BackEndLogs",
    autoCreateSqlTable: true,
    columnOptions:columnOptions
    )
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});


builder.Host.UseSerilog(log);




builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});





//Genel servilerimizi entegre ettiğimiz alan.
builder.Services.AddStorage<LocalStorage>();
builder.Services.AddInfrastructureServices();   //Infrastructure kısmında içinde ServisRegistration class'ını tanımı 
builder.Services.AddPersistanceServices();      //Persistance kısmında içinde ServisRegistration class'ını tanımı 
builder.Services.AddApplicationServices();      //Application kısmında içinde ServisRegistration class'ını tanımı 
builder.Services.AddSignalRServices();          //SignalR içinde ServisRegistration class'ını tanımı 



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
     ).AllowAnyHeader()//
     .AllowAnyMethod()//.
     .AllowCredentials()//.AllowCredentials() SignalR'ın çalışması için.
)) ;




//Authantication işlemleri

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin",options =>
    options.TokenValidationParameters = new()
    {
        //Oluşturulacak token değerini kimlerin/hangi originlerin/sitelerin kullanıcıya belirlediğimiz değerdir. -> www.bilmemne.com
        ValidateAudience = true,
        //Oluşturulacak token değerini kimin dağıttığını ifade edeceðimiz alandır. -> www.myapi.com
        ValidateIssuer = true,
        //Oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
        ValidateLifetime = true,
        //Üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden suciry key verisinin doğrulamasıdır.
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters)=>expires !=null ? expires>DateTime.UtcNow:false,

        /* 
         JWT üzerinden gelen Name Claim'e karşılık gelen değeri User.Identity.Name
        propertysinden elde edebilmek için
         */

        NameClaimType=ClaimTypes.Name
    }

);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";
    });
    app.UseCors();
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();

}


app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();


//Tabloya kullanıcının bilgilerini vermek için gerekli olan middleware düzenlemesi
app.Use(async (context,next) => {

    var userName = context.User.Identity.Name != null ? context.User.Identity.Name : null;

    using (LogContext.PushProperty("EmailOrUserNameLogs", userName))
    {
        //Bir sonraki middleware'a geçiş için
        await next();
    }

});


app.UseCors();
app.UseHttpsRedirection();


/*
 Bu middleware kendisinden sonra olan bütün işlemler için
loglama yapar  kendisinden önce gelen işlemler için loglama yapmaz

 */




//Kendi geliştirdiğimiz global Exception handler sınıfımız


app.UseSerilogRequestLogging();
app.UseHttpLogging(); //HTTP requestleri için middleware
app.UseAuthentication();
app.UseAuthorization();
app.MapHubs();          //SignalR için geliştirdiğimiz map işlemlerini yapacağımız fonk. tanımı



app.MapControllers();

app.Run();

