using System;
using OnionArch.Application.DTOs.UserDTOs;

namespace OnionArch.Application.Abstractions.UserServices
{
    // usernameOrEmail ve şifre kullanarak giriş yapılacak ekran.
    public interface IInternalAuthentication
	{
        Task<LoginUserResponseDTO> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
    }
}

