using MediatR;
using Microsoft.AspNetCore.Identity;
using OnionArch.Application.CustomExceptions;

namespace OnionArch.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        //UserManager servisi 
        //ıdentity tarafından oluşturulan yönetim işlemlerini yani kullanıcı işlemlerinden sorunlu olan servistir.
        //Bundan dolayı burada bizim repository vs oluşturmamıza gerek yoktu :) 

        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

          IdentityResult result =  await _userManager.CreateAsync(new()
            {
              //Gerekli olan ekleme işlemlerini burada gerçekleştiriyoruz.
                Id=Guid.NewGuid().ToString(),
                NameSurname=request.NameSurname,
                UserName =request.UserName,
                Email=request.Email,

                
                
            },request.Password);
            //Password'un en sona eklenme sebebi burada bir hash mantığı olması.

            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;



        }
    }
}

