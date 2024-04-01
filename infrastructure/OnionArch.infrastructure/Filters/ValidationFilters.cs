using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnionArch.Application.GlobalResponse;

namespace OnionArch.infrastructure.Filters
{
    public class ValidationFilters : IAsyncActionFilter
    {
        //Öncelikle bu işlem için IAsyncActionFilter 'i kalıtım yardımı ile almamı gerekmekte.
        //Daha sonra burada kalıtım yöntemiyle aldığımız ve konfigüre etmemiz gereken OnActionExecutionAsync
        ///fonksiyonunu implement etmemiz gerekmekte


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                     .Where(x => x.Value.Errors.Any())
                     .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                     .ToList();

                var response = new GlobalResponseResult
                {
                    Message = "Validation errors occurred on the backend.",
                    HassError = errors.Any() ? true : false, // Hata varsa true, yoksa false
                    ErrorMessage = errors.Any() ? string.Join(", ", errors) : null, // Hata mesajlarını birleştir
                    StatusCode = HttpStatusCode.BadRequest, // Hata durumunu belirt
                    StatusCodeString = HttpStatusCode.BadRequest.ToString() // Hata durumu string olarak belirt
                };

                context.Result = new BadRequestObjectResult(response);
                return;
            }

            await next();
        }
    }
}

