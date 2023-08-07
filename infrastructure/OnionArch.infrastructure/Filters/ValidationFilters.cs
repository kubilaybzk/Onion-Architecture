using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                       .ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage))
                       .ToArray();

                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            await next();
        }
    }
}

