using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using System.Text.Json;

namespace OnionArch.WebApi.Middlewares
{

    /*
    Şimdi burada ilk olarak oluşturdğumuz bu dosyanın program.cs içinde kullanabilecek şekle getirmemiz gerekiyor değil mi ?
    Bunun için öncelikle sınıfımızı static  olarak ayarlamamız gerekiyor.

    Bunu hallettikten hemen sonra program.cs içinde gerekli düzenleri yapmamız gerekmekte. 

     */
    static public class ConfigureExceptionHandlerExtension
    {

        /*artık program.cs içinde çağıracağımız middleware'ı burada bu fonksiyon içinde çağırıp gerekli olan
         güncellemeyi burada çağırıp burada konfigure edeceğiz. */
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {

            /* Bu program.cs içinde normal şartlarda çağıracağımız middleware bunu artık burada oluşturacağız. */
            application.UseExceptionHandler(builder =>
            {
                /*
                 Uygulamanın herhangi bir noktasında bir sorun olduğunda bunu yakalaycak olan fonskiyon.
                    Bu uygulama bizden bir delagete bekliyor.(üstüne tıklayarak görebiliriz)*/
                builder.Run(async context =>
                {
                    /*
                     Şimdi burada artık biz uygulamada herhangi bir yerde bir hata olduğu zaman uygulamızın kullanıcıya, ne olursa olsun
                        her türlü bir sonuç göndermesi gerekiyor değil mi ? 
                            işte burada o hata için düzenlemeler yapmaya başlayacağız 
                    
                    Şöyle bir yöntem izleyelim.
                        Hata kodunu server hatası olarak düzenleyelim.
                        Daha sonra hata mesajının türünü belirtelim /Text/Json/gibi 
                        
                    */

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // HTTP yanıtının durum kodunu ayarlar (Internal Server Error - HTTP durum kodu 500).
                    context.Response.ContentType = MediaTypeNames.Application.Json; // Yanıtın içerik türünü JSON olarak ayarlar.
                    // Bize context üzerinden gelen hata ile ilgili bütün bilgileri içeren bir interface. 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>(); 
                    if (contextFeature != null)
                    {
                        // Hata ayrıntılarını log'a kaydeder.
                        logger.LogError(contextFeature.Error.Message);
                        // Hata bilgilerini JSON formatında yanıt olarak gönderir.
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            // HTTP durum kodu.
                            StatusCode = context.Response.StatusCode,
                            // Hata mesajı.
                            Message = contextFeature.Error.Message,
                            // Başlık bilgisi.
                            Title = "Hata alındı!"
                        }));
                    }
                });
            });


            application.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            logger.LogError(contextFeature.Error.Message);

            var result = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = contextFeature.Error.Message,
                Title = "Hata alındı!"
            });

            await context.Response.WriteAsync(result);
        }
    });
});


        }
    }
}

