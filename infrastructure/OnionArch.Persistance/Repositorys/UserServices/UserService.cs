using System;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.DTOs.UserDTOs;
using OnionArch.Application.Features.Commands.AppUser.CreateUser;
using OnionArch.Domain.Entities.Identity;

namespace OnionArch.Persistance.Repositorys.UserServices
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDTO> CreateUser(CreateUserRequestDTO requestDTO)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                //Gerekli olan ekleme işlemlerini burada gerçekleştiriyoruz.
                //Artık bize veriler request üzerinden değil requestDTO üzerinden gelecek.
                Id = Guid.NewGuid().ToString(), 
                NameSurname = requestDTO.NameSurname,
                UserName = requestDTO.UserName,
                Email = requestDTO.Email,
            }, requestDTO.Password);
            //Password'un en sona eklenme sebebi burada bir hash mantığı olması.

            //Handler içinde CreateUserCommandResponse döndürüyorduk ama burada
            //CreateUserResponseDTO döndüreceğiz.

            CreateUserResponseDTO response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }
    }
}

