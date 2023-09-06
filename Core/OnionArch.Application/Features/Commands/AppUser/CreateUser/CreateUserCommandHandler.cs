using MediatR;
using Microsoft.AspNetCore.Identity;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.CustomExceptions;
using OnionArch.Application.DTOs.UserDTOs;

namespace OnionArch.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {

        // IUserService içinde yazdığımız kodu kullanacağız.

        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            CreateUserResponseDTO createUserResponseDTO = await _userService.CreateUser(new()
            {
                //    Gelen Tipi Persistance katmanına yollayacağımız için ve  Peristance içine göndereceğimiz tipin
                //        createUserRequestDTO olacağı için burada çevirme işlemi yapacağız.
                Email = request.Email,
                NameSurname=request.NameSurname,
                 Password=request.Password,
                 UserName=request.UserName
            });

            //Gidecek olan tip  createUserResponseDTO olacağı için burada çevirme işlemi yapacağız.
            return new()
            {
                Message = createUserResponseDTO.Message,
                Succeeded = createUserResponseDTO.Succeeded
            };


        }
    }
}

