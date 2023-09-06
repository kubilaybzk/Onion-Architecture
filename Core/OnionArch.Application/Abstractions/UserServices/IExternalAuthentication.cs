using System;
namespace OnionArch.Application.Abstractions.UserServices
{
    //Sosyal medya login  işlemleri ile ilgili tanımlar.

    public interface IExternalAuthentication
	{

        Task<DTOs.Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
        Task<DTOs.Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime);
    }
}

